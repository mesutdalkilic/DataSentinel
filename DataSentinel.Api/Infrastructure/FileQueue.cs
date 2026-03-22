using System.Collections.Concurrent;

namespace DataSentinel.Api.Infrastructure;

public class FileQueue : IFileQueue
{
    private readonly ConcurrentQueue<string> _filePaths = new();
    private readonly SemaphoreSlim _signal = new(0);

    public void Enqueue(string filePath)
    {
        _filePaths.Enqueue(filePath);
        _signal.Release();
    }

    public async Task<string> DequeueAsync(CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        _filePaths.TryDequeue(out var filePath);
        return filePath!;
    }
}