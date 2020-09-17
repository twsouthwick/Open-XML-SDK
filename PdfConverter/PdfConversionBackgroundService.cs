using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PdfConverter
{
    public static class PdfConverterExtensions
    {
        public static void AddPdfConverter(this IServiceCollection services)
        {
            services.AddHostedService<PdfConversionBackgroundService>();
            services.AddSingleton<ConversionQueue>();
            services.AddSingleton<IPdfConverter>(ctx => ctx.GetRequiredService<ConversionQueue>());
            services.AddSingleton<TaskMultiplier>();
        }
    }
    public interface IPdfConverter
    {
        Task<IPdfFile> ConvertAsync(Stream input, CancellationToken token);
    }

    public interface IPdfFile : IDisposable
    {
        Stream OpenStream();
    }

    internal class ConversionQueue : IPdfConverter
    {
        private readonly ConcurrentQueue<(Stream stream, TaskCompletionSource<IPdfFile> tcs)> _queue = new ConcurrentQueue<(Stream stream, TaskCompletionSource<IPdfFile>)>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public Task<IPdfFile> ConvertAsync(Stream input, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<IPdfFile>();
            _queue.Enqueue((input, tcs));
            _signal.Release();
            return tcs.Task;
        }

        public async Task<(Stream stream, TaskCompletionSource<IPdfFile> tcs)> DequeueAsync(CancellationToken token)
        {
            await _signal.WaitAsync(token);

            _queue.TryDequeue(out var result);
            return result;
        }
    }

    internal class PdfConversionBackgroundService : BackgroundService
    {
        private readonly ConversionQueue _queue;
        private readonly PdfConverter _converter;

        public PdfConversionBackgroundService(ConversionQueue queue, PdfConverter converter)
        {
            _queue = queue;
            _converter = converter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var pdf = new PdfConverter())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var (stream, tcs) = await _queue.DequeueAsync(stoppingToken);

                    try
                    {
                        var result = new TemporaryFile();

                        using (var file = await TemporaryFile.CreateAsync(stream))
                        {
                            pdf.Convert(file.Name, result.Name);
                        }

                        tcs.SetResult(result);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                }
            }
        }

        private class TemporaryFile : IPdfFile
        {
            public TemporaryFile()
                : this(".pdf")
            {
            }

            private TemporaryFile(string extension)
            {
                Name = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{extension}");
            }

            public static async Task<TemporaryFile> CreateAsync(Stream stream)
            {
                var file = new TemporaryFile(".docx");

                using (var fs = File.OpenWrite(file.Name))
                {
                    await stream.CopyToAsync(fs);
                }

                return file;
            }

            public string Name { get; }

            public void Dispose()
            {
                File.Delete(Name);
            }

            public Stream OpenStream() => File.OpenRead(Name);

            public Stream OpenWrite() => File.OpenWrite(Name);
        }
    }
}
