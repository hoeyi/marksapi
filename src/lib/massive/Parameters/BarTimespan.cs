namespace ApiClient.Massive
{
    /// <summary>
    /// Represents the size of the time window as required for price bar queries.
    /// </summary>
    public enum BarTimespan
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Second,

        Minute,

        Hour,

        Day,

        Week,

        Month,

        Quarter,

        Year
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}