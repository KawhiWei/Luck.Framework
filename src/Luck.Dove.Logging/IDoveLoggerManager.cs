namespace Luck.Dove.Logging;

public interface IDoveLoggerManager
{
    void Start();
    void Stop(CancellationToken cancellationToken);

    void Enqueue(string message);
}