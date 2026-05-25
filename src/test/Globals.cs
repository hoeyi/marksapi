global using System;
global using Xunit;

namespace ApiClient.Test
{
    /// <summary>
    /// Defines names of attributes applied to objects via TraitAttribute.
    /// </summary>
    record TestAttributeNames
    {
        public string Category { get; } = default!;
    }
}