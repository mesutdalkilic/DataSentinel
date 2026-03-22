namespace DataSentinel.Api.Infrastructure;

public interface IFileQueue
{
    void Enqueue(string filePath);
    Task<string> DequeueAsync(CancellationToken cancellationToken);
}