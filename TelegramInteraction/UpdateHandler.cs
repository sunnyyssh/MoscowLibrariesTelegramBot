using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramInteraction;

public sealed class UpdateHandler<TData, TRootTick> : IUpdateHandler
    where TRootTick : ITick<TData>, new()
{
    private readonly ConcurrentDictionary<long, DialogContext<TData>> _dialogs = new();
    
    private readonly ILogger _logger;

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        long? chatId = update.Message?.Chat.Id
                       ?? update.CallbackQuery?.Message?.Chat.Id
                       ?? null;
        
        if (chatId is null)
        {
            return;
        }

        await LogMessageAsync(chatId);
        
        if (!_dialogs.TryGetValue(chatId.Value, out var context))
        {
            var newRoot = new TRootTick();
            
            context = new DialogContext<TData>(chatId.Value, newRoot, _logger);
            
            _dialogs.TryAdd(chatId.Value, context);
        }

        if (context.CurrentTick is not null)
        {
            await context.CurrentTick.TickAsync(botClient, context, update);
        }

        async Task LogMessageAsync(ChatId id)
        {
            await _logger.LogInfoAsync($"New message in chat {id}");
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return _logger.LogErrorAsync(exception);
    }

    public UpdateHandler(ILogger logger)
    {
        _logger = logger;
    }
}