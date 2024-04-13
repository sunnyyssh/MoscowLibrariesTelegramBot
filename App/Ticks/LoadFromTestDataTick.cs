using Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramInteraction;
using File = System.IO.File;

namespace App;

internal class LoadFromTestDataTick : ITick<Library[]>
{
    private static readonly string TestDataPath = Path.Combine("TestData", "wifi-library.csv");

    private static readonly IDataProcessing<Library[]> DataProcessing =
        new CsvProcessing<Library>(new LibraryCsvSerializer());
    
    public async Task TickAsync(ITelegramBotClient botClient, DialogContext<Library[]> context, Update? update)
    {
        if (!File.Exists(TestDataPath))
        {
            await context.Logger.LogInfoAsync("Test data not found.");

            await botClient.SendTextMessageAsync(context.ChatId, "Test data not found.");
            
            context.TryPopTick();
            
            return;
        }

        var data = File.OpenRead(TestDataPath);

        data.Position = 0;
        context.BufferedData = await DataProcessing.ReadAsync(data);

        context.TryPopTick();
        if (context.CurrentTick is not null)
        {
            await context.CurrentTick.TickAsync(botClient, context, null);
        }
    }
}