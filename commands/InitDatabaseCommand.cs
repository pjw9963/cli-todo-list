using System.Diagnostics.CodeAnalysis;
using Productivity;
using Spectre.Console;
using Spectre.Console.Cli;

public class InitDatabaseCommand : Command<InitDatabaseCommand.Settings>
{
    public class Settings : CommandSettings { }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        using (var dbContext = new ItemStore())
        {
            dbContext.Database.EnsureCreated();

            var items = new List<Item>
            {
                new Item
                {
                    title = "Test task 1",
                    description = "This task is created for testing reasons.",
                    priority = Priority.ImportantForHealth,
                    schedule = Schedule.OneTime,
                    Updates = new List<ItemStatusUpdate>()
                },
                new Item
                {
                    title = "Test task 2",
                    description = "This is another task created for testing purposes.",
                    priority = Priority.Maintenance,
                    schedule = Schedule.Daily,
                    Updates = new List<ItemStatusUpdate>()
                    {
                        new ItemStatusUpdate { status = Productivity.Status.Done, UpdateTimestamp = DateTime.Now.AddDays(-3) },
                        new ItemStatusUpdate { status = Productivity.Status.InProgress, UpdateTimestamp = DateTime.Now.AddMinutes(-10) },
                        new ItemStatusUpdate { status = Productivity.Status.Done, UpdateTimestamp = DateTime.Now },
                    }
                },
                new Item
                {
                    title = "Test task 3",
                    description = "This is yet another task created for testing purposes.",
                    priority = Priority.Maintenance,
                    schedule = Schedule.Weekly,
                    Updates = new List<ItemStatusUpdate>()
                    {
                        new ItemStatusUpdate { status = Productivity.Status.Done, UpdateTimestamp = DateTime.Now.AddDays(-8) },
                        new ItemStatusUpdate { status = Productivity.Status.Done, UpdateTimestamp = DateTime.Now },
                    }
                }
            };

            dbContext.Items.AddRange(items);
            dbContext.SaveChanges();
        }

        AnsiConsole.WriteLine("Database created and populated with test data.");
        return 0;
    }

}