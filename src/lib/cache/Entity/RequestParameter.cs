using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ApiClient.Cache.Entity
{
    /// <summary>
        /// A single name/value pair. Immutable after construction; invalid instances cannot be created.
        /// </summary>
        public partial record RequestParameter
        {
            public const int MaxNameLength = 64;
            public const int MaxValueLength = 4000;

            // Required by EF Core for materialization.
            private RequestParameter()
            {
                Name = string.Empty;
                Value = string.Empty;
            }

            public RequestParameter(string name, string value, ParameterValueType valueType)
            {
                EnsureValidIdentifier(name, nameof(name));
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length > MaxValueLength)
                {
                    throw new ArgumentException(
                        $"Value exceeds the maximum length of {MaxValueLength} characters.", nameof(value));
                }

                if (value.Contains('\0'))
                {
                    throw new ArgumentException("Value cannot contain null characters.", nameof(value));
                }

                if (!Enum.IsDefined(valueType))
                {
                    throw new ArgumentOutOfRangeException(nameof(valueType));
                }

                if (!IsValidForType(value, valueType))
                {
                    throw new ArgumentException(
                        $"Value is not a valid {valueType} in invariant format.", nameof(value));
                }

                Name = name;
                Value = value;
                ValueType = valueType;
            }

            public string Name { get; private set; }

            public string Value { get; private set; }

            public ParameterValueType ValueType { get; private set; }

            internal static void EnsureValidIdentifier(string identifier, string paramName)
            {
                ArgumentNullException.ThrowIfNull(identifier, paramName);

                if (!IdentifierPattern().IsMatch(identifier))
                {
                    throw new ArgumentException(
                        $"Must start with a letter and contain only letters, digits, '_', '.' or '-' (max {MaxNameLength}).",
                        paramName);
                }
            }

            private static bool IsValidForType(string value, ParameterValueType valueType)
            {
                switch (valueType)
                {
                    case ParameterValueType.String:
                        return true;
                    case ParameterValueType.Integer:
                        return long.TryParse(value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out _);
                    case ParameterValueType.Decimal:
                        return decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out _);
                    case ParameterValueType.Boolean:
                        return bool.TryParse(value, out _);
                    case ParameterValueType.DateTime:
                        return DateTimeOffset.TryParseExact(value, "O", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                    default:
                        return false;
                }
            }

            // Allowlist; \z (not $) so a trailing newline cannot slip through. Timeout guards against ReDoS.
            [GeneratedRegex(@"\A[A-Za-z][A-Za-z0-9_.\-]{0,63}\z", RegexOptions.CultureInvariant, matchTimeoutMilliseconds: 100)]
            private static partial Regex IdentifierPattern();
        }

        /// <summary>
        /// Explicit type discriminator for a parameter value. Values are always stored as
        /// invariant-culture strings; no CLR type names are ever serialized.
        /// </summary>
        public enum ParameterValueType
        {
            String = 0,
            Integer = 1,
            Decimal = 2,
            Boolean = 3,
            DateTime = 4
        }
}

