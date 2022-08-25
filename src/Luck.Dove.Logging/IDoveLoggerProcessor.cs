namespace Luck.Dove.Logging;

public interface IDoveLoggerProcessor
{
    void Start();
    void Stop(CancellationToken cancellationToken);

    void Enqueue(string categoryName,string message);
}