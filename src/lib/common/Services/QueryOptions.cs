namespace ApiClient.Services
{
    /// <summary>
    /// Options for common query parameters.
    /// </summary>
    public class QueryOptions
    {
        /// <summary>
        /// Gets or sets the upper limit for records returned.
        /// </summary>
        public int UpperLimit { get; set; }

        /// <summary>
        /// Gets or sets the lower limit for records returned.
        /// </summary>
        public int LowerLimit { get; set; }
    }
}
