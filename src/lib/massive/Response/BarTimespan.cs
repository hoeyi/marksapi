namespace ApiClient.Massive.Response
{
    /// <summary>
    /// Represents the size of the time window as required for price bar queries.
    /// </summary>
    public enum BarTimespan
    {
        second,

        minute,

        hour,

        day,

        week,

        month,

        quarter,

        year
    }
}