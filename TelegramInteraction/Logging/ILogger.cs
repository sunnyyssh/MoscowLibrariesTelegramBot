namespace TelegramInteraction;

public interface ILogger
{
    void LogInfo(string message);

    Task LogInfoAsync(string message);

    void LogError(Exception exception, string? message = null);
    
    Task LogErrorAsync(Exception exception, string? message = null);
}