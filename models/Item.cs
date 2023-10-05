namespace Productivity;

public enum Priority
{
    ImportantForHealth,
    Maintenance,
    WouldBeCool
}

public enum Schedule
{
    OneTime,
    Daily,
    Weekly,
    Monthly
}

public enum Status
{
    Incomplete,
    InProgress,
    Done,
    Aborted
}

public class Item
{
    public Guid id { get; init; } = Guid.NewGuid();
    public string title { get; set; }
    public Priority priority { get; set; }
    public Schedule schedule { get; set; } = Schedule.OneTime;
    public string? description { get; set; }
    public TimeSpan? EstimatedDuration { get; set; }
    public TimeSpan? ActualDuration { get; set; }
    public List<ItemStatusUpdate> Updates { get; set; }
}

public class ItemStatusUpdate
{
    public Guid id { get; set; } = Guid.NewGuid();
    public Status status { get; set; }
    public DateTime UpdateTimestamp { get; set; } = DateTime.Now;
}