namespace TelegramInteraction;

public class FileLogger : StreamLogger
{
    public FileLogger(string fileName) 
        : base(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
    { }
}