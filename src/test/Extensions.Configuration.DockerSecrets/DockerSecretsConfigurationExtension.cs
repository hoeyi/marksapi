using System.IO;
using Microsoft.Extensions.Configuration;

namespace Extensions.Configuration.DockerSecrets
{
    /// <summary>
    /// Extension methods for registering <see cref="DockerSecretsConfigurationProvider"/> and 
    /// <see cref="DockerSecretsConfigurationSource"/>.
    /// </summary>
    public static class DockerSecretsConfigurationExtension
    {
        public static IConfigurationBuilder AddDockerSecrets(this IConfigurationBuilder configurationBuilder)
        {
            return AddDockerSecrets(configurationBuilder, DockerSecretsConfigurationProvider.DefaultSecretsPath);
        }

        public static IConfigurationBuilder AddDockerSecrets(this IConfigurationBuilder configurationBuilder, string secretsPath, Action<string> handle = null)
        {
            configurationBuilder.Add(new DockerSecretsConfigurationSource(secretsPath, handle));
            return configurationBuilder;
        }
}
}
