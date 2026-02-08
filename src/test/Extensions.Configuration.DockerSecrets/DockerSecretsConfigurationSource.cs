using System.IO;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.DockerSecrets
{
    /// <summary>
    /// Represents a Docker Secrets source of configuration key/values for an application.
    /// </summary>
    public class DockerSecretsConfigurationSource : IConfigurationSource
    {
        private readonly string _secretsPath;
        private readonly Action<string>? _handle;

        public DockerSecretsConfigurationSource(string secretsPath) : this(secretsPath, null) {}

        public DockerSecretsConfigurationSource(string secretsPath, Action<string>? handle)
        {
            _secretsPath = secretsPath ?? throw new ArgumentNullException(nameof(secretsPath));
            _handle = handle;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DockerSecretsConfigurationProvider(_secretsPath, _handle);
        }
    }
}
