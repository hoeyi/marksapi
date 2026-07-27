using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;

namespace Ichyd.Marksapi.Cli.Services
{
    /// <summary>
    /// Handles writing output.
    /// </summary>
    public class OutputService
    {
        [ExcludeFromCodeCoverage]
        private static JsonSerializerOptions _jsonOptions { get; } = JsonOptions();

        [ExcludeFromCodeCoverage]
        private static FileStreamOptions FileOptions { get; } = FileStreamOptions();
        
        /// <summary>
        /// Writes the given <typeparamref name="T"/> data to disk at the given path and format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The record(s) to write.</param>
        /// <param name="format">One of (json, csv). Case insensitive.</param>
        /// <param name="path">The file path to write output to.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task"/> containing a count of bytes written.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> was not one of allowed values.</exception>
        /// <exception cref="ArgumentException"><paramref name="format"/> or <paramref name="path"/> were null or empty.</exception>
        public static async Task<double> WriteAsync<T>(
            T[] data,
            string format,
            string path,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(format);
            ArgumentException.ThrowIfNullOrWhiteSpace(format);

            ArgumentException.ThrowIfNullOrEmpty(path);
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            var normalizedFormat = format.ToLowerInvariant();

            if (normalizedFormat is not ("json" or "csv" or "console"))
                throw new ArgumentOutOfRangeException(
                    nameof(format), $"Unsupported format '{format}'. Supported formats: json, csv, console");


            return normalizedFormat switch
            {
                "json" => await WriteJsonAsync(path, data, cancellationToken),
                "csv" => await WriteCsvAsync(path, data, cancellationToken),
                "console" => await Task.Run(() => WriteConsole(data), cancellationToken),
                _ => throw new NotImplementedException("D"),
            };
        }

        /// <summary>
        /// Writes the given <typeparamref name="T"/> data to disk at the given path and format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The record(s) to write.</param>
        /// <param name="format">One of (json, csv, console). Case insensitive.</param>
        /// <param name="path">The file path to write output to.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>A <see cref="Task"/> containing a count of bytes written.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> was not one of allowed values.</exception>
        /// <exception cref="ArgumentException"><paramref name="format"/> or <paramref name="path"/> were null or empty.</exception>
        public static async Task<double> WriteAsync<T>(
            T item,
            string format,
            string path,
            CancellationToken cancellationToken = default)
                => await WriteAsync<T>(data: [item], format, path, cancellationToken);

        [ExcludeFromCodeCoverage]
        private static async Task<double> WriteJsonAsync<T>(
            string path,
            T[] data,
            CancellationToken cancellationToken)
        {
            string jsonPath = $"{path}.json";
            CheckPathOrThrow(jsonPath);

            cancellationToken.ThrowIfCancellationRequested();

            var serialized = JsonSerializer.Serialize(data, _jsonOptions);

            using var writer = new StreamWriter(jsonPath, Encoding.UTF8, options: FileOptions);
            
            await writer.WriteAsync(
                new StringBuilder(serialized), cancellationToken);

            return writer.BaseStream.Length;
        }

        public static string CombinePath(params string[] paths) => Path.Combine(paths);

        [ExcludeFromCodeCoverage]
        private static async Task<double> WriteCsvAsync<T>(
            string path,
            T[] data,
            CancellationToken cancellationToken)
        {
            CheckPathOrThrow(path);

            cancellationToken.ThrowIfCancellationRequested();
            
            using var writer = new StreamWriter(path, Encoding.UTF8, options: FileOptions);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(data, cancellationToken);

            return writer.BaseStream.Length;
        }

        [ExcludeFromCodeCoverage]
        private static double WriteConsole<T>(T[] data)
        {
            var serialized = JsonSerializer.Serialize(data, _jsonOptions);
            Console.Write(serialized);

            return Encoding.UTF8.GetByteCount(serialized);
        }
        
        [ExcludeFromCodeCoverage]
        private static void CheckPathOrThrow(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if(string.IsNullOrEmpty(directory))
                throw new InvalidOperationException($"Could not find part of the path: {path}");

            if(!File.Exists(path))
                return;
            
            FileAttributes fileAttr = File.GetAttributes(path);
            if(fileAttr.HasFlag(FileAttributes.Directory))
                throw new InvalidOperationException(
                            $"Parameter '{nameof(path)}' must be a file, not directory.");
        }

        [ExcludeFromCodeCoverage]
        private static JsonSerializerOptions JsonOptions() => new()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        
        [ExcludeFromCodeCoverage]
        private static FileStreamOptions FileStreamOptions() => new()
            {
                Mode = FileMode.OpenOrCreate,
                Access = FileAccess.ReadWrite
            };
    }
}