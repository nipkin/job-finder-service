using JobFinder.Application.JobPostings;
using System.Collections.Concurrent;

namespace JobFinder.Infrastructure.JobPostings
{
    public class JobMatchRunner : IJobMatchRunner
    {
        private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _running = new();

        public bool TryStart(Guid userId, out CancellationTokenSource cts)
        {
            cts = new CancellationTokenSource();
            if (_running.TryAdd(userId, cts))
                return true;

            cts.Dispose();
            cts = null!;
            return false;
        }

        public bool Stop(Guid userId)
        {
            if (!_running.TryRemove(userId, out var cts))
                return false;

            cts.Cancel();
            cts.Dispose();
            return true;
        }

        public bool IsRunning(Guid userId) => _running.ContainsKey(userId);

        public void Complete(Guid userId)
        {
            if (_running.TryRemove(userId, out var cts))
                cts.Dispose();
        }
    }
}
