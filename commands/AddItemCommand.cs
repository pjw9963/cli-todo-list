using System.Diagnostics.CodeAnalysis;
using Productivity;
using Spectre.Console.Cli;

public class AddItemCommand : Command<AddItemCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<title>")]
        public string Title { get; set; }

        [CommandArgument(1, "<priority>")]
        public Priority Priority { get; set; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var item = new Item()
        {
            title = settings.Title,
            priority = settings.Priority,
        };
        var itemManager = ItemManager.Instance;
        itemManager.AddItem(item);
        return 0;
    }

}