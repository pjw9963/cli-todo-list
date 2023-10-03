using Microsoft.EntityFrameworkCore;
using Productivity;

public class ItemStore : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source=D:\ItemStoreContext.db;");
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<ItemStatusUpdate> ItemUpdates { get; set; }
}