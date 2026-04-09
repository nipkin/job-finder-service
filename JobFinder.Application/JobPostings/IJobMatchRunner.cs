namespace JobFinder.Application.JobPostings
{
    public interface IJobMatchRunner
    {
        bool TryStart(Guid userId, out CancellationTokenSource cts);
        bool Stop(Guid userId);
        bool IsRunning(Guid userId);
        void Complete(Guid userId);
    }
}
