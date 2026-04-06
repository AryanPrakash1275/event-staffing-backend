namespace EventStaffing.Api.Models;

public class Volunteer
{
    public int Id { get; set; }
    public string Phone { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Application> Applications { get; set; } = new();
}