using Ichyd.Extensions.Configuration.Docker;
using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ApiClient.Massive;
using ApiClient.Services;
using Ichyd.Marksapi.Cli.Extensions;
using Ichyd.Marksapi.Cli.Massive.Verbs;
using Ichyd.Marksapi.Cli.Services;
using Ichyd.Marksapi.Cli.Verbs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using Spectre.Console;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Ichyd.Marksapi.Cli
{
    [ExcludeFromCodeCoverage]

    class Program
    {
        internal const string MASSIVE_API_KEYPATH = "MASSIVE_API_KEY";

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
        /// Gets the program <see cref="IServiceProvider"/>.
        /// </summary>
        public static IServiceProvider Services { get; set; } = default!;

        static Task<int> Main(string[] args)
        {
            RootCommand rootCommand = default!;
            try
            {
                Configuration = InitConfiguration();
                Logger = InitLogger<Program>(Configuration);


                Logger.LogInfo_Services_Initializing();

                MassiveApi = InitMassiveApi(Configuration, Logger);
                QueryLimit = Configuration
                                .GetQueryOptionsOrDefault()
                                .QueryLimit();

                var provider = new SingletonServiceProvider();
                provider
                    .RegisterService(Configuration)
                    .RegisterService(Logger);

                if (Configuration.GetSection("massive").Exists())
                {
                    provider
                        .RegisterService(InitMassiveApi(Configuration, Logger));

                    Logger.LogInfo_Service_Registered(nameof(MassiveApi));

                }
                
                Services = provider;

                Logger.LogInfo_Services_Initializing_Finished();

                rootCommand = InitRootCommand(Configuration);

                var parse = rootCommand.Parse(args);
                rootCommand.SetAction(DoRootCommand);
            }
            catch(Exception e)
            {
                Console.Error.Write($"Error during startup.\n\n{e.Message}\n");
                #if DEBUG
                Console.Error.Write($"\n{e.StackTrace}");
                #endif
                Environment.Exit(1);
            }

            if(rootCommand is null)
                throw new InvalidOperationException("Failed to initial program. No root command parsed.");
            return rootCommand.Parse(args).InvokeAsync();
        }

        private static int DoRootCommand(ParseResult parseResult)
        {
            AnsiConsole.Write(new FigletText(nameof(Marksapi).ToLower()));
            AnsiConsole.WriteLine(
                $"\n{parseResult.RootCommandResult.Command.Description!}");
            
            return 0;
        }

        #region Initializers
        private static IMassiveApi InitMassiveApi(IConfiguration configuration, ILogger? logger = null)
        {
            RateOptions options = new();
            var section = configuration
                .GetSection("massive")
                .GetSection(nameof(RateOptions));
            if (section is null)
            {
                options.Limit = 5;
                options.Interval = 60;
            }
            else
                section.Bind(options);

            ArgumentException.ThrowIfNullOrWhiteSpace(configuration[MASSIVE_API_KEYPATH]);
            
            return new MassiveApi(
                configuration[MASSIVE_API_KEYPATH]!,
                rateOptions: options,
                logger: logger);
        }

        private static IConfiguration InitConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
            #if DEBUG // for Debug, Docker secrets is not expected to work.
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.debug.json");
            #else // for non-Debug, we expect Docker secrets will be used.
                .AddDockerSecrets()
                .AddJsonFile("appsettings.json");
            #endif

            return configBuilder.Build();
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


        private static RootCommand InitRootCommand(IConfiguration configuration)

        {
            var rootCommand = new RootCommand()
            {
                FileReaderHandler.CreateLicenseCommand(),
                FileReaderHandler.CreateNoticeCommand()
            };
            rootCommand.Description = "A unified command line interface for querying financial data APIs.";

            // Massive service subcommand
            var massiveCommand = new Command("massive", "Access Massive API endpoints")
            {
                TickerHandler.CreateCommand(),
                TickerInfoHandler.CreateCommand(),
                AggregateBarHandler.CreateCommand(),
                ShortVolumeHandler.CreateCommand(),
                InflationHandler.CreateCommand(),
                LaborHandler.CreateCommand(),
                TreasuryHandler.CreateCommand()
            };

            rootCommand.Add(massiveCommand);
            for (int i = 0; i < rootCommand.Options.Count; i++)
            {
                if (rootCommand.Options[i] is VersionOption)
                    rootCommand.Options[i].Action = new CustomVersionAction();
            }

            return rootCommand;
        }
        #endregion
    }
}
