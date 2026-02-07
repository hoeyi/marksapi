#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace ApiClient.Marketstack
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    class LoggingTemplates
    {
        public class Error
        {
            public const string HttpErrorGeneral = "Error requesting or receiving response.\n {Exception}";

            public const string InvalidOrEmptyResponse = "Response was empty or invalid.";
        }
    }
}