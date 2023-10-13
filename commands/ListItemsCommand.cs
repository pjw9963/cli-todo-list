using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Productivity;
using Spectre.Console;
using Spectre.Console.Cli;

public class ListItemsCommand : Command<ListItemsCommand.Settings>
{
    private readonly ItemStore _itemStore;
    struct listResult
    {
        public Item Item;
        public ItemStatusUpdate? Update;
    }

    public ListItemsCommand(ItemStore itemStore)
    {
        _itemStore = itemStore;
    }

    public class Settings : CommandSettings
    {        
        [CommandOption("-s|--status")]
        [Description("List items only with the provided status")]
        public Productivity.Status? Status { get; set; }

        [CommandOption("-t|--timeline")]
        [Description("Provide a timeline to list items that have the provided status on the provided timeline. If no status provided, defaults to Done")]
        public Productivity.Schedule? Timeline { get; set; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        // todo -- update function so status is determined by schedule 
        // so we want a list of items that match the specified status
        // currently we just grab the latest update
        // i think we check to see if that update is on current day/week/month
        var itemQuery = _itemStore.Items.Select(i => new listResult
        {
            Item = i,
            Update = i.Updates
                    .OrderByDescending(u => u.UpdateTimestamp)
                    .First()
        });        

        if (settings.Status != null && settings.Status != Productivity.Status.Incomplete)
        {
            itemQuery = itemQuery.Where(lr => lr.Update != null && lr.Update.status == settings.Status);
        }

        if (settings.Status == null || settings.Status == Productivity.Status.Incomplete)
        {
            itemQuery = itemQuery.Where(lr => lr.Update != null || (lr.Update != null && lr.Update.status == Productivity.Status.Incomplete));
        }

        if (settings.Timeline != null)
        {
            itemQuery = itemQuery.Where(lr => lr.Item.schedule == settings.Timeline);
        }

        var items = itemQuery.ToList();

        var table = new Table();

        // Add some columns
        table.AddColumn("Id");
        table.AddColumn("Title");
        table.AddColumn("Priority");
        table.AddColumn("Status");
        table.AddColumn("Schedule");

        // Add some rows
        foreach (listResult lr in items)
        {
            var item = lr.Item;
            var status = lr.Update?.status;
            var shortId = item.id.ToString().Split('-')[0];

            var statusColor = "blue";
            if (status == Productivity.Status.InProgress) statusColor = "green";
            if (status == Productivity.Status.Aborted) statusColor = "red";
            if (status == Productivity.Status.Done) statusColor = "grey";

            var priorityColor = "blue";
            if (item.priority == Priority.ImportantForHealth) priorityColor = "red";
            if (item.priority == Priority.Maintenance) priorityColor = "yellow";
            if (item.priority == Priority.WouldBeCool) priorityColor = "purple";

            table.AddRow($"[grey]{shortId}[/]", $"[blue]{item.title}[/]", $"[{priorityColor}]{item.priority}[/]", $"[{statusColor}]{status}[/]", $"{item.schedule}");
        }

        // Render the table to the console
        AnsiConsole.Write(table);
        return 0;
    }

}