using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<AddItemCommand>("add-item");
    config.AddCommand<ListItemsCommand>("list-items");
});

app.Run(args);
