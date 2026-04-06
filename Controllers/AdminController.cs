using EventStaffing.Api.Data;
using EventStaffing.Api.DTOs;
using EventStaffing.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventStaffing.Api.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("events")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        var eventItem = new Event
        {
            Title = request.Title.Trim(),
            Location = request.Location.Trim(),
            City = request.City.Trim(),
            EventDate = request.EventDate,
            ShiftStart = request.ShiftStart,
            ShiftEnd = request.ShiftEnd,
            PayPerDay = request.PayPerDay,
            Description = request.Description.Trim(),
            RequiredVolunteers = request.RequiredVolunteers,
            Status = "Open",
            CreatedAt = DateTime.UtcNow
        };

        _db.Events.Add(eventItem);
        await _db.SaveChangesAsync();

        return Ok(eventItem);
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _db.Events
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

        return Ok(events);
    }

    [HttpGet("events/{eventId:int}/applications")]
    public async Task<IActionResult> GetEventApplications(int eventId)
    {
        var eventExists = await _db.Events.AnyAsync(e => e.Id == eventId);

        if (!eventExists)
        {
            return NotFound(new { message = "Event not found." });
        }

        var applications = await _db.Applications
            .Where(a => a.EventId == eventId)
            .Include(a => a.Volunteer)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new
            {
                a.Id,
                a.EventId,
                a.VolunteerId,
                a.Status,
                a.AppliedAt,
                Volunteer = new
                {
                    a.Volunteer.Id,
                    a.Volunteer.Name,
                    a.Volunteer.Phone,
                    a.Volunteer.City
                }
            })
            .ToListAsync();

        return Ok(applications);
    }

    [HttpPatch("applications/{id:int}/status")]
    public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] UpdateApplicationStatusRequest request)
    {
        if (request.Status != "Approved" && request.Status != "Rejected")
        {
            return BadRequest(new { message = "Status must be Approved or Rejected." });
        }

        var application = await _db.Applications
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application is null)
        {
            return NotFound(new { message = "Application not found." });
        }

        application.Status = request.Status;
        await _db.SaveChangesAsync();

        return Ok(application);
    }
}

public class UpdateApplicationStatusRequest
{
    public string Status { get; set; } = null!;
}