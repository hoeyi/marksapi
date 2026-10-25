using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ApiClient.Cache.Entity
{
    /// <summary>
    /// A structured request with a variable set of named parameters.
    /// </summary>
    public record StructuredRequest
    {
        public const int MaxParameters = 100;

        public StructuredRequest(Guid id, string command, DateTimeOffset createdAt)
        {
            ArgumentException.ThrowIfNullOrEmpty(command);

            Id = id;
            Command = command;
            CreatedAt = createdAt;

            Parameters = [];
        }

        public Guid Id { get; set; }

        public string Command { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        // Mapped as an EF Core 10 complex collection (JSON column).
        // Each element validates itself on construction.
        public List<RequestParameter> Parameters { get; private set; }

        public void AddParameter(RequestParameter parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter);

            if (Parameters.Count >= MaxParameters)
            {
                throw new InvalidOperationException(
                    $"A request cannot contain more than {MaxParameters} parameters.");
            }

            if (Parameters.Any(p => string.Equals(p.Name, parameter.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    $"Parameter '{parameter.Name}' already exists on this request.");
            }

            Parameters.Add(parameter);
        }

        public bool RemoveParameter(string name)
        {
            return Parameters.RemoveAll(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase)) > 0;
        }

        public RequestParameter? FindParameter(string name)
        {
            return Parameters.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));
        }
    }    
}