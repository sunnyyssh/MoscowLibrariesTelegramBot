using Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramInteraction;

namespace App;

public class FilteringTick : ITick<Library[]>
{
    private const string AskingValueMessage = "Send me a value";

    private readonly IFilter<Library> _filter;

    private bool _isWaitingValue;

    public async Task TickAsync(ITelegramBotClient botClient, DialogContext<Library[]> context, Update? update)
    {
        if (context.BufferedData is null)
        {
            await botClient.SendTextMessageAsync(context.ChatId, "There is no data.");
            context.TryPopTick();
            if (context.CurrentTick is not null)
            {
                await context.CurrentTick.TickAsync(botClient, context, null);
            }
            return;
        }
        
        if (!_isWaitingValue || update is null)
        {
            await botClient.SendTextMessageAsync(context.ChatId, AskingValueMessage);
            _isWaitingValue = true;
            return;
        }

        if (update?.Message?.Text is null)
        {
            await botClient.SendTextMessageAsync(context.ChatId, "Where is the value?");
            return;
        }

        if (!_filter.TryFilterBy(context.BufferedData, update.Message.Text, out var filtered))
        {
            await botClient.SendTextMessageAsync(context.ChatId, "What the value did you send... Try again");
            return;
        }

        context.BufferedData = filtered.ToArray();
        
        context.TryPopTick();
        if (context.CurrentTick is not null)
        {
            await context.CurrentTick.TickAsync(botClient, context, null);
        }
    }

    public FilteringTick(IFilter<Library> filter)
    {
        _filter = filter;
    }
}