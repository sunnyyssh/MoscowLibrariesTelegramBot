using Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInteraction;
using File = Telegram.Bot.Types.File;

namespace App;

public sealed class SendDataTick : ITick<Library[]>
{
    private bool _isWaitingFormat;

    private Dictionary<string, IDataProcessing<Library[]>> _dataProcessings = new()
    {
        { ".csv", new CsvProcessing<Library>(new LibraryCsvSerializer()) },
        { ".json", new JsonProcessing<Library[]>() }
    };

    public async Task TickAsync(ITelegramBotClient botClient, DialogContext<Library[]> context, Update? update)
    {
        var chatId = context.ChatId;

        if (context.BufferedData is null)
        {
            await botClient.SendTextMessageAsync(chatId, "No data is buffered. You should load it somewhere.");
            
            context.TryPopTick();
            
            if (context.CurrentTick is not null)
            {
                await context.CurrentTick.TickAsync(botClient, context, null);
            }
            return;
        }

        if (!_isWaitingFormat)
        {
            await SendFormatChoseAsync(botClient, chatId);

            _isWaitingFormat = true;
            
            return;
        }
        
        if (update?.CallbackQuery is null) 
            return;

        if (update.CallbackQuery.Message is not null)
        {
            await botClient.EditMessageReplyMarkupAsync(chatId, update.CallbackQuery.Message.MessageId);
        }
        
        var format = update.CallbackQuery.Data;

        if (format is null || !_dataProcessings.TryGetValue(format, out var dataProcessing)) 
            return;

        _isWaitingFormat = false;
        await using var stream = await dataProcessing.WriteAsync(context.BufferedData);
        
        var file = InputFile.FromStream(stream, GenerateFileName(format));
        
        await botClient.SendDocumentAsync(chatId, file);

        context.TryPopTick();
            
        if (context.CurrentTick is not null)
        {
            await context.CurrentTick.TickAsync(botClient, context, null);
        }
    }

    private string GenerateFileName(string format)
    {
        return $"data-{DateTime.Now.Millisecond}{format}";
    }

    private Task SendFormatChoseAsync(ITelegramBotClient botClient, ChatId chatId)
    {
        var markup = CreateMarkup();
        return botClient.SendTextMessageAsync(chatId, "Choose format...", replyMarkup: markup);
    }

    private InlineKeyboardMarkup CreateMarkup()
    {
        var buttons = _dataProcessings.Keys
            .Select(InlineKeyboardButton.WithCallbackData);

        var markup = new InlineKeyboardMarkup(buttons);

        return markup;
    }
}