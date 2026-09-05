namespace ApiClient.Massive.Parameters
{
    /// <summary>
    /// Represents a comparison modifier for numeric and derived types.
    /// </summary>
    public enum NumericComparisonOperator
    {

        /// <summary>
        /// Any of a given collection (typically used for strict equality).
        /// </summary>
        Any,

        /// <summary>
        /// Greater than.
        /// </summary>
        Gt,

        /// <summary>
        /// Greater than or equal to.
        /// </summary>
        Gte,

        /// <summary>
        /// Less than.
        /// </summary>
        Lt,

        /// <summary>
        /// Less than or equal to.
        /// </summary>
        Lte
    }
}

