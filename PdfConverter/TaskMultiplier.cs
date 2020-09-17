using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PdfConverter
{
    public class TaskMultiplierSettings
    {
        public int MaxCount { get; set; }
    }

    public class TaskMultiplier
    {
        private readonly ILogger<TaskMultiplier> _logger;

        public TaskMultiplier(ILogger<TaskMultiplier> logger)
        {
            _logger = logger;
        }

        public Task RunAsync(Func<CancellationToken, Task> task, TaskMultiplierSettings settings, CancellationToken token)
        {
            return Task.Run(() => RunAsyncInternal(task, settings, token), token);
        }

        private async Task RunAsyncInternal(Func<CancellationToken, Task> work, TaskMultiplierSettings settings, CancellationToken token)
        {
            var count = 0;

            using (var state = new State(settings.MaxCount, _logger))
            {
                _id.Value = state.Id;

                while (!token.IsCancellationRequested)
                {
                    await state.WaitAsync(token);

                    state.Add(Task.Run(() =>
                    {
                        var id = Interlocked.Increment(ref count);
                        _taskId.Value = id;
                        state.Logger.LogInformation("Adding task {TaskId}", id);
                        return work(token);
                    }, token));
                }
            }
        }

        private static readonly AsyncLocal<Guid> _id = new AsyncLocal<Guid>();
        private static readonly AsyncLocal<int> _taskId = new AsyncLocal<int>();

        public static Guid CurrentId => _id.Value;

        public static int TaskId => _taskId.Value;

        private sealed class State : IDisposable
        {
            private readonly IDisposable _scope;
            private readonly ConcurrentDictionary<Task, byte> _collection;
            private readonly SemaphoreSlim _semaphore;

            public State(int desiredCount, ILogger logger)
            {
                Logger = logger;
                _scope = Logger.BeginScope("Task multiplier {Id}", Id);
                _collection = new ConcurrentDictionary<Task, byte>();
                _semaphore = new SemaphoreSlim(desiredCount);
            }

            public Guid Id => Guid.NewGuid();

            public ILogger Logger { get; }

            public void Add(Task task)
            {
                if (_collection.TryAdd(task, 0))
                {
                    _ = task.ContinueWith(Cleanup);
                }
            }

            private void Cleanup(Task t) => Remove(t);

            public void Remove(Task task)
            {
                Logger.LogInformation("Task {TaskId} ended with status {TaskStatus}", TaskId, task.Status);

                _collection.TryRemove(task, out _);
                _semaphore.Release();
            }

            public Task WaitAsync(CancellationToken token) => _semaphore.WaitAsync(token);

            public void Dispose()
            {
                _scope.Dispose();
                _semaphore.Dispose();
            }
        }
    }
}
