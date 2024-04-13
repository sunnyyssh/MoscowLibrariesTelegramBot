using System.Runtime.Serialization;
using Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramInteraction;

namespace App;

public sealed class ReceivingDocumentTick : ITick<Library[]>
{
    private readonly string _format;

    private readonly string _askingMessage;

    private static readonly string NotDocumentMessage = "But it's not even a document. I'm getting upset.";

    private static readonly string IncorrectFormatMessage = "It has incorrect format. I'm losing all the patience!";

    private readonly IDataProcessing<Library[]> _dataProcessing;

    private bool _isWaitingDocument;

    public async Task TickAsync(ITelegramBotClient botClient, DialogContext<Library[]> context, Update? update)
    {
        if (!_isWaitingDocument || update is null)
        {
            await botClient.SendTextMessageAsync(context.ChatId, _askingMessage);
            
            _isWaitingDocument = true;
            return;
        }

        if (update.Message?.Document is null)
        {
            await botClient.SendTextMessageAsync(context.ChatId, NotDocumentMessage);
            return;
        }
        
        if (Path.GetExtension(update.Message.Document.FileName) != _format)
        {
            await botClient.SendTextMessageAsync(context.ChatId, IncorrectFormatMessage);
            return;
        }

        var file = await botClient.GetFileAsync(update.Message.Document.FileId);

        if (file.FilePath is null)
        {
            await context.Logger.LogInfoAsync("File path is null");
            return;
        }

        var stream = new MemoryStream();

        await botClient.DownloadFileAsync(file.FilePath, stream);

        stream.Position = 0;
        context.BufferedData = await _dataProcessing.ReadAsync(stream);

        _isWaitingDocument = false;

        context.TryPopTick();
        
        if (context.CurrentTick is not null)
        {
            await context.CurrentTick.TickAsync(botClient, context, null);
        }
    }

    public ReceivingDocumentTick(string format, IDataProcessing<Library[]> dataProcessing)
    {
        _format = format;
        _dataProcessing = dataProcessing;
        _askingMessage = $"Hmm.. Okay, then send me {_format} file";
    }
}