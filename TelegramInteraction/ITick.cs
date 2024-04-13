using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramInteraction;

public interface ITick<TData>
{
    public Task TickAsync(ITelegramBotClient botClient, DialogContext<TData> context, Update? update);
}