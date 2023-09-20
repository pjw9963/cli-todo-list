using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<AddItemCommand>("add-item");
    config.AddCommand<ListItemsCommand>("list-items");
    config.AddCommand<PickItemCommand>("pick-item");
});

app.Run(args);
