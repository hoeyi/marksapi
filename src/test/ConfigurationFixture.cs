
using Microsoft.Extensions.Configuration;

namespace ApiClient.Marketstack.xUnitTests;
    public class ConfigurationFixture : IDisposable
    {
        public ConfigurationFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<ConfigurationFixture>()
                .Build();
        }
        
        public IConfiguration Configuration { get; }

        public void Dispose() => GC.SuppressFinalize(this);
    }