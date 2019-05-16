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
    public interface IConversionQueue
    {
        Task<Stream> ConvertAsync(Stream input, CancellationToken token);
    }

    internal class ConversionQueue : IConversionQueue
    {
        private readonly ConcurrentQueue<(Stream stream, TaskCompletionSource<Stream> tcs)> _queue = new ConcurrentQueue<(Stream stream, TaskCompletionSource<Stream>)>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public Task<Stream> ConvertAsync(Stream input, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<Stream>();
            _queue.Enqueue((input, tcs));
            _signal.Release();
            return tcs.Task;
        }

        public async Task<(Stream stream, TaskCompletionSource<Stream> tcs)> DequeueAsync(CancellationToken token)
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
            while (!stoppingToken.IsCancellationRequested)
            {
                var (stream, tcs) = await _queue.DequeueAsync(stoppingToken);


            }
        }
    }
}
