namespace EventStaffing.Api.DTOs;

public class CreateEventRequest
{
    public string Title { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string City { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public TimeSpan ShiftStart { get; set; }
    public TimeSpan ShiftEnd { get; set; }
    public decimal PayPerDay { get; set; }
    public string Description { get; set; } = null!;
    public int RequiredVolunteers { get; set; }
}