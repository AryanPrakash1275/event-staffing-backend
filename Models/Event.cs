namespace EventStaffing.Api.Models;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public TimeSpan ShiftStart { get; set; }
    public TimeSpan ShiftEnd { get; set; }
    public decimal PayPerDay { get; set; }
    public string Description { get; set; } = null!;
    public int RequiredVolunteers { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Application> Applications { get; set; } = new();
}