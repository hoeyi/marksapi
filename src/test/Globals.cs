global using System;
global using Xunit;

namespace ApiClient.Marketstack.xUnitTests
{
    /// <summary>
    /// Defines names of attributes applied to objects via TraitAttribute.
    /// </summary>
    internal record TestAttributeNames
    {
        public string Category { get; } = default!;
    }
}