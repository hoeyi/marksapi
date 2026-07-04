using System;
using System.CommandLine;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli
{
    /// <summary>
    /// Handles writing output.
    /// </summary>
    public class OutputService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        private static readonly FileStreamOptions _fileOptions = new()
        {
            Mode = FileMode.Append
        };

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

            if (normalizedFormat is not ("json" or "csv"))
                throw new ArgumentOutOfRangeException(
                    nameof(format), $"Unsupported format '{format}'. Supported formats: json, csv");


            return normalizedFormat switch
            {
                "json" => await WriteJsonAsync(path, data, cancellationToken),
                "csv" => await WriteCsvAsync(path, data, cancellationToken),
                _ => throw new NotImplementedException("D"),
            };
        }

        /// <summary>
        /// Writes the given <typeparamref name="T"/> data to disk at the given path and format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The record(s) to write.</param>
        /// <param name="format">One of (json, csv). Case insensitive.</param>
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

        private static async Task<double> WriteJsonAsync<T>(string path, T[] data, CancellationToken cancellationToken)
        {
            string jsonPath = $"{path}.json";
            CheckPathOrThrow(jsonPath);

            cancellationToken.ThrowIfCancellationRequested();

            var serialized = JsonSerializer.Serialize(data, _jsonOptions);

            using var writer = new StreamWriter(jsonPath, Encoding.UTF8, options: _fileOptions);
            
            await writer.WriteAsync(
                new StringBuilder(serialized), cancellationToken);

            return writer.BaseStream.Length;
        }

        private static async Task<double> WriteCsvAsync<T>(string path, T[] data, CancellationToken cancellationToken)
        {
            CheckPathOrThrow(path);

            cancellationToken.ThrowIfCancellationRequested();
            
            using var writer = new StreamWriter(path, Encoding.UTF8, options: _fileOptions);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(data, cancellationToken);

            return writer.BaseStream.Length;
        }

        private static void CheckPathOrThrow(string path)
        {
            FileAttributes fileAttr = File.GetAttributes(path);
            if(fileAttr.HasFlag(FileAttributes.Directory))
                throw new InvalidOperationException(
                            $"Parameter '{nameof(path)}' must be a file, not directory.");
        }
    }
}