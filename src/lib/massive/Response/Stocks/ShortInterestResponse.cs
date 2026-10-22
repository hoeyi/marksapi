using ApiClient.Massive.Response.Generic;

namespace ApiClient.Massive.Response.Stocks
{
    /// <summary>
    /// Represents the response from the Massive API endpoint for retrieving short interest data.
    /// </summary>
    public class ShortInterestResponse : CollectionResponse<ShortInterestDetail>
    {
    }
}