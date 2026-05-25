namespace ApiClient.Massive.Response
{
    /// <summary>
    /// Represents the size of the time window as required for price bar queries.
    /// </summary>
    readonly struct BarTimespan
    {
        public const string Second = "second";

        public const string Minute = "minute";

        public const string Hour ="hour";

        public const string Day = "day";

        public const string Week = "week";

        public const string Month = "month";

        public const string Quarter = "quarter";

        public const string Year = "year";
    }

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