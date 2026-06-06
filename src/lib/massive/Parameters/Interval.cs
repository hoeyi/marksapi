using System;
using System.Diagnostics.CodeAnalysis;

namespace ApiClient.Massive;

[ExcludeFromCodeCoverage]
/// <summary>
/// Represents a mathematical interval for <typeparamref name="T"/> in single dimension.
/// </summary>
/// <typeparam name="T">The base type for the interval.</typeparam>
public readonly record struct Interval<T>
    where T : IEquatable<T>
{
    /// <summary>
    /// Represents the staring value of the interval.
    /// </summary>
    public T Start { get; init; }

    /// <summary>
    /// Indicates the interval includes only values greater than the value of <see cref="Start"/>.
    /// </summary>
    public bool OpenLeft { get; init; }

    /// <summary>
    /// Represents the terminal value of the interval.
    /// </summary>
    public T End { get; init; }

    /// <summary>
    /// Indicates the interval includes only values less than the value of <see cref="End"/>.
    /// </summary>
    public bool OpenRight { get; init; }

    /// <summary>
    /// Flag indicating the interval is open left/right.
    /// </summary>
    public readonly bool IsOpen => OpenLeft && OpenRight;
}
