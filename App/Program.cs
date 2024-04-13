#define WEIRD_IMPLEMENTATION

using Data;

using TelegramInteraction;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace App;

public static class Program
{
    private const string DefaultApiKeyPath = ".apikey";

    public static void Main()
    {
        Console.WriteLine();
        
        // READ README.md !!!!!
        
        var key = RecieveKey();
        
        var botOptions = new TelegramBotClientOptions(key.Key);
        var receiverOptions = new ReceiverOptions();
        var logger = InitializeLogger();
        
        var botHandler = new BotDataHandler<Library[], GreetingTick>(botOptions, receiverOptions, logger);

        botHandler.Start(CancellationToken.None);
        
        botHandler.Wait();
    }

    private static ILogger InitializeLogger()
    {
        const string folderName = "var";
        
        string fileName = $"logging_session_{DateTime.Now:G}"
            .Replace('/', '_')
            .Replace(' ', '_')
            .Replace(':', '_');

#if WEIRD_IMPLEMENTATION
        // If it's done how task says logger must log to directory with .csproj file.
        // I don't know why it's done
        // because information about running program (in my opinion) should be placed at program level.
        string LevelsLowerDir(int n) => Path.Combine(Enumerable.Repeat("..", n).ToArray());

        var dirPath = Path.Combine(LevelsLowerDir(3), folderName);

#else
        // Here it is how it should be.
        var dirPath = folderName;
#endif
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        var filePath = Path.Combine(dirPath, fileName);
        
        return new FileLogger(filePath);
    }

    private static ApiKey RecieveKey()
    {
        string? path = DefaultApiKeyPath;

        ApiKey? result;
        
        while (!ApiKey.TryLoadFrom(path, out result))
        {
            Console.WriteLine("Enter path to api key file.");
            path = Console.ReadLine();
        }

        return result;
    }
}