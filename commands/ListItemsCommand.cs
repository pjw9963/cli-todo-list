using System.Diagnostics.CodeAnalysis;
using Productivity;
using Spectre.Console;
using Spectre.Console.Cli;

public class ListItemsCommand : Command<ListItemsCommand.Settings>
{
    private readonly ItemStore _itemStore;
    public ListItemsCommand(ItemStore itemStore) {
        _itemStore = itemStore;
    }
    public class Settings : CommandSettings
    { }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var items = _itemStore.Items.ToList();

        var table = new Table();

        // Add some columns
        table.AddColumn("Id");
        table.AddColumn("Title");
        table.AddColumn("Priority");
        table.AddColumn("Status");

        // Add some rows
        foreach (Item item in items)
        {
            var shortId = item.id.ToString().Split('-')[0];

            var statusColor = "blue";
            // if (item.status == Productivity.Status.InProgress) statusColor = "green";
            // if (item.status == Productivity.Status.Aborted) statusColor = "red";
            // if (item.status == Productivity.Status.Done) statusColor = "grey";

            var priorityColor = "blue";
            if (item.priority == Priority.ImportantForHealth) priorityColor = "red";
            if (item.priority == Priority.Maintenance) priorityColor = "yellow";
            if (item.priority == Priority.WouldBeCool) priorityColor = "purple";

            table.AddRow($"[grey]{shortId}[/]", $"[blue]{item.title}[/]", $"[{priorityColor}]{item.priority}[/]", $"[{statusColor}]WIP[/]");
        }

        // Render the table to the console
        AnsiConsole.Write(table);
        return 0;
    }

}