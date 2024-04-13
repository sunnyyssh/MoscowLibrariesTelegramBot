using System.Collections.Concurrent;
using Telegram.Bot.Types;

namespace TelegramInteraction;

public sealed class DialogContext<TData>
{
    public ILogger Logger { get; }
    
    public ChatId ChatId { get; }

    public TData? BufferedData { get; set; }

    private readonly ConcurrentStack<ITick<TData>> _ticksStack = new();

    public ITick<TData>? CurrentTick => _ticksStack.TryPeek(out var result) ? result : null;
    
    public void PushTick(ITick<TData> nextTick)
    {
        _ticksStack.Push(nextTick);
    }

    public bool TryPopTick()
    {
        return _ticksStack.TryPop(out _);
    }
    
    public DialogContext(long chatId, ITick<TData> rootTick, ILogger logger)
    {
        ChatId = chatId;
        _ticksStack.Push(rootTick);
        Logger = logger;
    }
}