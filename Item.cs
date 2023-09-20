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
    public Guid id { get; init; }
    public string title { get; set; }
    public Priority priority { get; set; }
    public Schedule schedule { get; set; } = Schedule.OneTime;
    public string? description { get; set; }
    public Status status { get; set; } = Status.Incomplete;
    public TimeSpan? EstimatedDuration { get; set; }
    public TimeSpan? ActualDuration { get; set; }

    public Item(string title, Priority priority)
    {
        this.id = Guid.NewGuid();
        this.title = title;
        this.priority = priority;
    }
}