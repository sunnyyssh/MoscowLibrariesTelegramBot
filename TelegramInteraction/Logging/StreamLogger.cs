using System.Text;

namespace TelegramInteraction;

public class StreamLogger : ILogger, IDisposable, IAsyncDisposable
{
    private readonly StreamWriter _writer;

    public void LogError(Exception exception, string? message = null)
    {
        string logMessage = $"ERROR:\t({DateTime.Now:G})\t\"{message}\"";

        _writer.WriteLine(logMessage);
    }

    public Task LogErrorAsync(Exception exception, string? message = null)
    {
        string logMessage = $"ERROR:\t({DateTime.Now:G})\t\"{message}\"";

        return _writer.WriteLineAsync(logMessage);
    }

    public void LogInfo(string message)
    {
        string logMessage = $"INFO: \t({DateTime.Now:G})\t\"{message}\"";
        
        _writer.WriteLine(logMessage);
    }

    public Task LogInfoAsync(string message)
    {
        string logMessage = $"INFO: \t({DateTime.Now:G})\t\"{message}\"";

        return _writer.WriteLineAsync(message);
    }

    public void Dispose()
    {
        _writer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _writer.DisposeAsync();
    }

    public StreamLogger(Stream stream)
    {
        if (!stream.CanWrite)
        {
            throw new ArgumentException("Stream should be able to write.", nameof(stream));
        }

        _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
    }
}