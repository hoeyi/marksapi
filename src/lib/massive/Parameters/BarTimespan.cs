namespace ApiClient.Massive
{
    /// <summary>
    /// Represents the size of the time window as required for price bar queries.
    /// </summary>
    public enum BarTimespanEnum
    {
        Second,

        Minute,

        Hour,

        Day,

        Week,

        Month,

        Quarter,

        Year
    }
}