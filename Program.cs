using Injection.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

var registrations = new ServiceCollection(); // I think this is dumb

registrations.AddSingleton(new AppConfig
{
    ConnectionString = "connection-string"
});

registrations.AddDbContext<ItemStore>();

// Create a type registrar and register any dependencies.
// A type registrar is an adapter for a DI framework.
var registrar = new TypeRegistrar(registrations);

var app = new CommandApp(registrar);
app.Configure(config =>
{
    config.AddCommand<InitDatabaseCommand>("init-db");
    config.AddCommand<AddItemCommand>("add-item");
    config.AddCommand<ListItemsCommand>("list-items");
    config.AddCommand<PickItemCommand>("pick-item");
    config.AddCommand<ItemDetailsCommand>("item-details");
});

app.Run(args);