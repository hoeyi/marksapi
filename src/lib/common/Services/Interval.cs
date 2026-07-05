using System;
using System.Diagnostics.CodeAnalysis;

namespace ApiClient.Services;

[ExcludeFromCodeCoverage]
/// <summary>
/// Represents a mathematical interval for <typeparamref name="T"/> in single dimension.
/// </summary>
/// <typeparam name="T">The base type for the interval.</typeparam>
public readonly record struct Interval<T>
    where T : struct, IEquatable<T>, IComparable<T>
{
    public Interval()
    {
    }

    public Interval(T min, T max, bool open = false)
    {
        Start = min;
        End = max;
        OpenLeft = open;
        OpenRight = open;
    }

    public bool Contains(T other)
    {
        bool testLeft = OpenLeft ? 
            IsGreater(other, Start) : 
            IsGreaterOrEqual(other, Start);
        bool testRight = OpenRight ? 
            IsLess(other, End) : 
            IsLessOrEqual(other, End);
        
        return testLeft && testRight;
    }
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

    private static bool IsGreater(T a, T b) => a.CompareTo(b) > 0;
    private static bool IsGreaterOrEqual(T a, T b) => 
        a.Equals(b) || a.CompareTo(b) > 0;
    private static bool IsLess(T a, T b) => a.CompareTo(b) < 0;
    private static bool IsLessOrEqual(T a, T b) => 
        a.Equals(b) || a.CompareTo(b) < 0;
}
