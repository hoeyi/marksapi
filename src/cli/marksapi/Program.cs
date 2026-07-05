using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using ApiClient.Services;
using Marksapi.Cli.Massive.Verbs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Marksapi.Cli
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        /// <summary>
        /// Gets the <see cref="IConfiguration"/> instance for this program.
        /// </summary>
        static IConfiguration Configuration { get; set; } = default!;
        
        static IMassiveApi MassiveApi  { get; set; } = default!;
        
        /// <summary>
        /// Gets the default <see cref="ILogger"/> for the program.
        /// </summary>
        static ILogger Logger  { get; set; } = default!;

        /// <summary>
        /// Gets the program-constrained limits for records to return. Default [1, 5000].
        /// </summary>
        public static Interval<int> QueryLimit { get; set; }

        /// <summary>
        /// Gets the program <see cref="IServierProvider"/>.
        /// </summary>
        public static IServiceProvider Services { get; set; } = default!;

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

            try
            {
                Configuration = InitConfiguration();
                MassiveApi = InitApi(Configuration);
                Logger = InitLogger<Program>(Configuration);
                QueryLimit = GetQueryOptionsOrDefault(Configuration);

                var provider = new SingletonServiceProvider();
                provider
                    .RegisterService(Configuration)
                    .RegisterService(Logger)
                    .RegisterService(MassiveApi);
                Services = provider;
            }
            catch(Exception e)
            {
                Console.Error.Write($"Error during startup.\n\n{e.Message}\n");
                #if DEBUG
                Console.Error.Write($"\n{e.StackTrace}");
                #endif
                Environment.Exit(1);
            }

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

        private static Interval<int> GetQueryOptionsOrDefault(IConfiguration configuration)
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

            return new Interval<int>(options.LowerLimit, options.UpperLimit, open: false);
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
