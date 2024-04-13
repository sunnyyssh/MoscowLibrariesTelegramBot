using Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramInteraction;

namespace App;

public sealed class SortingTick : ITick<Library[]>
{
    private readonly ISorter<Library> _sorter;
    
    public async Task TickAsync(ITelegramBotClient botClient, DialogContext<Library[]> context, Update? update)
    {
        if (context.BufferedData is null)
        {
            return;
        }

        context.BufferedData = _sorter.Sort(context.BufferedData).ToArray();

        context.TryPopTick();
        
        if (context.CurrentTick is {} currentTick)
        {
            await currentTick.TickAsync(botClient, context, null);
        }
    }

    public SortingTick(ISorter<Library> sorter)
    {
        _sorter = sorter;
    }
}