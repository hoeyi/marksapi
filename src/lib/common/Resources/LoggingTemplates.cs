namespace ApiClient.Resources
{
    public sealed class LoggingTemplates
    {
        public class Error
        {
            public const string HttpErrorGeneral = "Error requesting or receiving response.\n {Exception}";

            public const string InvalidOrEmptyResponse = "Response was empty or invalid.";
        }
    }
}