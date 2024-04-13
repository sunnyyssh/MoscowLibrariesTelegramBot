using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInteraction;

namespace App;

public class ReplyMarkupTick<TData> : ITick<TData>
{
    private readonly IReplyMarkup _markup;

    private readonly string _markupSendingMessage;
    
    private readonly string _wrongReplyMessage;

    private readonly string _afterMarkupMessage;
    
    private readonly bool _popAfterMarkup;

    private readonly IReadOnlyDictionary<string, Lazy<ITick<TData>>> _handlers;

    private bool _isWaitingReply;

    public async Task TickAsync(ITelegramBotClient botClient, DialogContext<TData> context, Update? update)
    {
        if (!_isWaitingReply || update is null)
        {
            await botClient.SendTextMessageAsync(context.ChatId, _markupSendingMessage, replyMarkup: _markup);
            
            _isWaitingReply = true;
            return;
        }
        
        if (update?.Message?.Text is null || 
            !_handlers.TryGetValue(update.Message.Text, out var lazyTick))
        {        
            await botClient.SendTextMessageAsync(context.ChatId, _wrongReplyMessage);
            
            return;
        }

        await botClient.SendTextMessageAsync(context.ChatId, _afterMarkupMessage,
            replyMarkup: new ReplyKeyboardRemove());
        
        if (_popAfterMarkup)
        {
            context.TryPopTick();
        }
        
        context.PushTick(lazyTick.Value);

        _isWaitingReply = true;
        
        await lazyTick.Value.TickAsync(botClient, context, null);
    }
    public ReplyMarkupTick(string markupSendingMessage, string wrongReplyMessage, string afterMarkupMessage, 
        bool popAfterMarkup, IReadOnlyDictionary<string, Lazy<ITick<TData>>> handlers)
    {
        _markupSendingMessage = markupSendingMessage;
        _wrongReplyMessage = wrongReplyMessage;
        _afterMarkupMessage = afterMarkupMessage;
        _popAfterMarkup = popAfterMarkup;
        _handlers = handlers;

        var buttons = handlers.Keys.Select(text => new KeyboardButton(text));

        _markup = new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true };
    }
}