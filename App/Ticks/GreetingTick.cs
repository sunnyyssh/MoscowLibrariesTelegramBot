using System.Diagnostics.SymbolStore;
using Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInteraction;

namespace App;

public class GreetingTick : ITick<Library[]>
{
    private const string GreetingMessage = "Hey, I'm .json and .csv professional parser, sorter and selector. I hope we'll be friends :)";

    private static readonly IReadOnlyDictionary<string, Lazy<ITick<Library[]>>> DocumentReceivingContinuations =
        new Dictionary<string, Lazy<ITick<Library[]>>>()
        {
            {
                ".csv",
                new Lazy<ITick<Library[]>>(() =>
                    new ReceivingDocumentTick(".csv",
                        new CsvProcessing<Library>(new LibraryCsvSerializer())))
            },
            {
                ".json",
                new Lazy<ITick<Library[]>>(() =>
                    new ReceivingDocumentTick(".json",
                        new JsonProcessing<Library[]>()))
            },
        };

    private static readonly IReadOnlyDictionary<string, Lazy<ITick<Library[]>>> SortDataContinuations = 
        Library.Sorters
        .Select(pair =>
            KeyValuePair.Create(
                pair.Key,
                new Lazy<ITick<Library[]>>(() =>
                    new SortingTick(pair.Value))))
        .ToDictionary(pair => pair.Key, pair => pair.Value);

    private static readonly IReadOnlyDictionary<string, Lazy<ITick<Library[]>>> FilterDataContinuations = 
        Library.Filter
        .Select(pair =>
            KeyValuePair.Create(
                pair.Key,
                new Lazy<ITick<Library[]>>(() =>
                    new FilteringTick(pair.Value))))
        .ToDictionary(pair => pair.Key, pair => pair.Value);

    private static readonly IReadOnlyDictionary<string, Lazy<ITick<Library[]>>> Continuations =
        new Dictionary<string, Lazy<ITick<Library[]>>>()
        {
            {
                "Load from document",
                new Lazy<ITick<Library[]>>(() =>
                    new ReplyMarkupTick<Library[]>(
                        "Choose format of document you want to send...",
                        "Uh.. Do you hear me?",
                        "Oh.. You're good",
                        true,
                        DocumentReceivingContinuations))
            },
            { "Load from test data", new Lazy<ITick<Library[]>>(() => new LoadFromTestDataTick()) },
            { "Send data", new Lazy<ITick<Library[]>>(() => new SendDataTick()) },
            {
                "Sort data",
                new Lazy<ITick<Library[]>>(() =>
                    new ReplyMarkupTick<Library[]>(
                        "Choose option to sort with",
                        "It's not possible to sort by this propterty",
                        "Okay, I'll sort.",
                        true,
                        SortDataContinuations))
            },
            {
                "Filter data",
                new Lazy<ITick<Library[]>>(() =>
                    new ReplyMarkupTick<Library[]>(
                        "What property to filter by?",
                        "It's not one of given properies",
                        "Property was choosen",
                        true,
                        FilterDataContinuations))
            }
        };

    public async Task TickAsync(ITelegramBotClient botClient, DialogContext<Library[]> context, Update? update)
    {
        await botClient.SendTextMessageAsync(context.ChatId, GreetingMessage);
        
        var menuTick = new ReplyMarkupTick<Library[]>(
            "Choose the option, my dear friend...",
            "It seems you missed hahaha",
            "Wow... You're awesome...",
            false,
            Continuations);
        
        context.PushTick(menuTick);

        await menuTick.TickAsync(botClient, context, null);
    }
}