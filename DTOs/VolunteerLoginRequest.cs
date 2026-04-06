namespace EventStaffing.Api.DTOs;

public class VolunteerLoginRequest
{
    public string Phone { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
}