namespace EventStaffing.Api.Models;

public class Application
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int VolunteerId { get; set; }
    public Volunteer Volunteer { get; set; } = null!;

    public string Status { get; set; } = "Pending";
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}