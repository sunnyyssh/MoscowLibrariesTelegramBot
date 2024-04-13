using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramInteraction;

public sealed class BotDataHandler<TData, TRootTick>
    where TRootTick : ITick<TData>, new()
{
    private readonly AutoResetEvent _waitEvent = new(true);
    
    private readonly ReceiverOptions _receiverOptions;

    private readonly UpdateHandler<TData, TRootTick> _updateHandler;
    
    private readonly ITelegramBotClient _botClient;

    public void Start(CancellationToken cancellationToken)
    {
        _botClient.StartReceiving(
            _updateHandler.HandleUpdateAsync,
            _updateHandler.HandlePollingErrorAsync,
            _receiverOptions,
            cancellationToken);
        
        // Waiting threads will continue if cancelled.
        cancellationToken.Register(() => _waitEvent.Set());
        // Enforce waiting.
        _waitEvent.Reset();
    }

    public void Wait()
    {
        _waitEvent.WaitOne();
    }
    
    public BotDataHandler(TelegramBotClientOptions botOptions, ReceiverOptions receiverOptions, ILogger logger)
    {
        _receiverOptions = receiverOptions;
        _updateHandler = new UpdateHandler<TData, TRootTick>(logger);
        _botClient = new TelegramBotClient(botOptions);
    }
}