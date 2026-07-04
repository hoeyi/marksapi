using System.CommandLine;
using ApiClient.Massive;
using ApiClient.Services;
using Marksapi.Cli.Massive.Verbs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Marksapi.Cli
{
    
    public class Program
    {
        static IConfiguration Configuration = InitConfiguration();
        static IMassiveApi MassiveApi = InitApi(Configuration);
        static ILogger Logger = InitLogger<Program>(Configuration);
        public static QueryOptions QueryLimit = 
            GetQueryOptionsOrDefault(Configuration);
        static Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("A unified command line interface for querying financial data APIs.");

            // Massive service subcommand
            var massiveCommand = new Command("massive", "Access Massive API endpoints")
            {
                TickerHandler.CreateCommand(),
                TickerInfoHandler.CreateCommand(),
                AggregateBarHandler.CreateCommand(),
                ShortVolumeHandler.CreateCommand()
            };

            rootCommand.Add(massiveCommand);
            var parse = rootCommand.Parse(args);
            rootCommand.SetAction((args, cancellationToken) =>
            {
                return DoRootCommand(args, cancellationToken);
            });

            Configuration = InitConfiguration();

            return rootCommand.Parse(args).InvokeAsync();
        }

        private static Task<int> DoRootCommand(
            ParseResult parseResult,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private static MassiveApi InitApi(IConfiguration configuration)
        {
            RateOptions options = new();
            var section = configuration
                .GetSection("massive")?
                .GetSection(nameof(RateOptions));
            if (section is null)
            {
                options.Limit = 5;
                options.Interval = 60;
            }
            else
                section.Bind(options);

            ArgumentException.ThrowIfNullOrWhiteSpace(configuration["massive:api_key"]);
            
            return new MassiveApi(configuration["massive:api_key"]!, rateOptions: options);
        }

        private static QueryOptions GetQueryOptionsOrDefault(IConfiguration configuration)
        {   
            QueryOptions options = new();
            var section = configuration
                .GetSection("massive")?
                .GetSection(nameof(QueryOptions));
            if (section is null)
            {
                options.UpperLimit = 5000;
                options.LowerLimit = 1;
            }
            else
                section.Bind(options);

            return options;
        }
        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json")
                .Build();
            
            return config;
        }
        
        private static ILogger InitLogger<T>(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            var logConfig = new LoggerConfiguration();

            if(configuration is not null)
                logConfig.ReadFrom.Configuration(configuration);
            else
                logConfig
                    .WriteTo.Console()
                    .WriteTo.File("logs/.log", rollingInterval: RollingInterval.Day)
                    .WriteTo.File(new CompactJsonFormatter(), "logs/.json", rollingInterval: RollingInterval.Day);

            Log.Logger = logConfig.CreateLogger();

            var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);

            return loggerFactory.CreateLogger<T>();
        }

    }
}
