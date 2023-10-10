using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

public class ItemDetailsCommand : Command<ItemDetailsCommand.Settings>
{
    private readonly ItemStore _itemStore;

    public ItemDetailsCommand(ItemStore itemStore)
    {
        _itemStore = itemStore;
    }

    public class Settings : CommandSettings { }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var choices = _itemStore.Items.Select(item => new
        {
            Item = item,
            LatestStatusUpdate = item.Updates
                    .OrderByDescending(update => update.UpdateTimestamp)
                    .FirstOrDefault()
        })
        .Select(x => $"{x.Item.id.ToString()} - {x.Item.title} - {(x.LatestStatusUpdate != null ? x.LatestStatusUpdate.status.ToString() : Productivity.Status.Incomplete.ToString())}")
        .ToList();

        var pickedItemId = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Select an item to view all updates.")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                .AddChoices(choices)).Split(' ')[0];

        var table = new Table();

        table.AddColumn("status");
        table.AddColumn("timestamp");

        var updates = _itemStore.ItemUpdates.Where(i => i.itemId.ToString() == pickedItemId);

        if (updates == null)
            return 1;

        foreach (var update in updates)
        {
            table.AddRow(update.status.ToString(), update.UpdateTimestamp.ToString());
        }

        AnsiConsole.MarkupLine($"Showing updates for item [bold]{pickedItemId}[/]");
        AnsiConsole.Write(table);
        return 0;
    }
}