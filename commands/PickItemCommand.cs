using System.Diagnostics.CodeAnalysis;
using Productivity;
using Spectre.Console;
using Spectre.Console.Cli;

public class PickItemCommand : Command<PickItemCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-s|--status")]
        public Productivity.Status Status { get; set; } = Productivity.Status.InProgress;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var itemManager = ItemManager.Instance;
        // Ask for the user's favorite fruit

        var choices = itemManager.GetItems()
        .Where(x => x.status != settings.Status && x.status != Productivity.Status.Done)
        .Select(x => $"{x.id.ToString().Split('-')[0]} - {x.title} - {x.status}");

        var pickedItem = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Select an item to change the status to [green]{settings.Status}[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                .AddChoices(choices));

        Item item = itemManager.GetItem(pickedItem.Split(' ')[0]);
        item.status = settings.Status;
        itemManager.UpdateItem(item);

        AnsiConsole.WriteLine($"Item {item.id} has been updated to {settings.Status}");
        return 0;
    }
}