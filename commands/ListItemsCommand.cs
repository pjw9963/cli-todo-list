using Productivity;
using Spectre.Console;
using Spectre.Console.Cli;

public class ListItemsCommand : Command<ListItemsCommand.Settings>
{
    public class Settings : CommandSettings
    { }

    public override int Execute(CommandContext context, Settings settings)
    {
        var itemManager = ItemManager.Instance;
        var items = itemManager.GetItems();

        var table = new Table();

        // Add some columns
        table.AddColumn("Title");
        table.AddColumn("Priority");
        table.AddColumn("Status");

        // Add some rows
        foreach (Item item in items)
        {
            table.AddRow($"[blue]{item.title}[/]", $"[blue]{item.priority}[/]", $"[blue]{item.status}[/]");
        }

        // Render the table to the console
        AnsiConsole.Write(table);
        return 0;
    }

}