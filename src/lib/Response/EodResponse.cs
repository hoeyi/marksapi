using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the resonse body from the <b><em>/eod</em></b> endpoint.
    /// </summary>
    public class EodResponse
    {
        /// <summary>
        /// Gets or sets the <b><em>data</em></b> of the response.
        /// </summary>
        [JsonProperty("data")]
        public EodData[] Data { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Pagination"> data of the response.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }
}