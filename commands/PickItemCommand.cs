using System.Diagnostics.CodeAnalysis;
using Productivity;
using Spectre.Console;
using Spectre.Console.Cli;

public class PickItemCommand : Command<PickItemCommand.Settings>
{
    private readonly ItemStore _itemStore;

    public PickItemCommand(ItemStore itemStore)
    {
        _itemStore = itemStore;
    }

    public class Settings : CommandSettings
    {
        [CommandOption("-s|--status")]
        public Productivity.Status Status { get; set; } = Productivity.Status.InProgress;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var choices = _itemStore.Items.Select(item => new
        {
            Item = item,
            LatestStatusUpdate = item.Updates
                    .OrderByDescending(update => update.UpdateTimestamp)
                    .Select(update => update.status)
                    .FirstOrDefault()
        })
        .Where(x => x.LatestStatusUpdate != settings.Status)
        .Select(x => $"{x.Item.id.ToString()} - {x.Item.title} - {x.LatestStatusUpdate}")
        .ToList();

        var pickedItemId = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Select an item to change the status to [green]{settings.Status}[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                .AddChoices(choices)).Split(' ')[0];

        //item.status = settings.Status;
        var item = _itemStore.Items.FirstOrDefault(i => i.id.ToString() == pickedItemId);

        if (item == null) return 1;

        item.Updates.Add(new ItemStatusUpdate
        {
            status = settings.Status
        });
        _itemStore.SaveChanges();

        AnsiConsole.WriteLine($"Item {item.id} has been updated to {settings.Status}");
        return 0;
    }
}