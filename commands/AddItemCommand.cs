using System.Diagnostics.CodeAnalysis;
using Productivity;
using Spectre.Console.Cli;

public class AddItemCommand : Command<AddItemCommand.Settings>
{
    private readonly ItemStore _itemStore;

    public AddItemCommand(ItemStore itemStore)
    {
        _itemStore = itemStore;
    }

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
        _itemStore.Items.Add(item);
        _itemStore.SaveChanges();
        return 0;
    }

}