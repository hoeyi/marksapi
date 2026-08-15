using Ichyd.Marksapi.Cli.Extensions;
using Ichyd.Marksapi.Cli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Ichyd.Marksapi.Cli.Verbs
{
    [ExcludeFromCodeCoverage]
    static class FileReaderHandler
    {
        public static Command CreateLicenseCommand()
        {
            var command = new Command("--license", "Display this application license")
                {
                    Aliases = {"-l"}
                };
            command.SetAction((pr, ct) => 
            {
                return Task.Run(() => Handle(Program.Services, "LICENSE"), ct);
            });

            return command;
        }

        public static Command CreateNoticeCommand()
        {
            var command = new Command("--notice", "Display third-party library info from NOTICE file")
                {
                    Aliases = {"-n"}
                };
            command.SetAction((pr, ct) => 
            {
                return Task.Run(() => Handle(Program.Services, "NOTICE"), ct);
            });

            return command;
        }

        private static readonly FileStreamOptions _fileStreamOptions = new()
        {
            Mode = FileMode.Open,
            Access = FileAccess.Read
        };

        /// <inheritdoc/>
        private static void Handle(IServiceProvider serviceProvider, string filename)
        {
            ArgumentException.ThrowIfNullOrEmpty(filename);

            var logger = serviceProvider.GetRequiredService<ILogger>();
            var appDirectory =
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            string path = Path.Combine(appDirectory ?? "./", filename);
            if(!File.Exists(path))
            {
                logger.LogWarning_FileNotExists(path);
                Console.WriteLine("Could not locate NOTICE file.");
                return;
            }

            using var sr = new StreamReader(path, _fileStreamOptions);

            while(!sr.EndOfStream)
            {
                for(int i = 0; i < 100; i++)
                {
                    if(sr.EndOfStream)
                        break;
                
                    Console.WriteLine(sr.ReadLine());
                }

                Console.WriteLine("Press q to quit, or enter to continue");
                if(Console.ReadKey().Key == ConsoleKey.Q)
                    break;
            }
        }
    }
}
