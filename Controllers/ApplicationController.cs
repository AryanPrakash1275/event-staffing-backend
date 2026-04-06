using EventStaffing.Api.Data;
using EventStaffing.Api.DTOs;
using EventStaffing.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventStaffing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ApplicationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] ApplyRequest request)
    {
        var volunteer = await _db.Volunteers
            .FirstOrDefaultAsync(v => v.Id == request.VolunteerId);

        if (volunteer is null)
        {
            return BadRequest(new { message = "Volunteer not found." });
        }

        var eventItem = await _db.Events
            .FirstOrDefaultAsync(e => e.Id == request.EventId);

        if (eventItem is null)
        {
            return BadRequest(new { message = "Event not found." });
        }

        if (eventItem.Status != "Open")
        {
            return BadRequest(new { message = "Applications are only allowed for open events." });
        }

        var alreadyApplied = await _db.Applications
            .AnyAsync(a => a.EventId == request.EventId && a.VolunteerId == request.VolunteerId);

        if (alreadyApplied)
        {
            return BadRequest(new { message = "Volunteer has already applied to this event." });
        }

        var application = new Application
        {
            EventId = request.EventId,
            VolunteerId = request.VolunteerId,
            Status = "Pending",
            AppliedAt = DateTime.UtcNow
        };

        _db.Applications.Add(application);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            application.Id,
            application.EventId,
            application.VolunteerId,
            application.Status,
            application.AppliedAt
        });
    }

    [HttpGet("volunteer/{volunteerId:int}")]
    public async Task<IActionResult> GetByVolunteer(int volunteerId)
    {
        var volunteerExists = await _db.Volunteers
            .AnyAsync(v => v.Id == volunteerId);

        if (!volunteerExists)
        {
            return NotFound(new { message = "Volunteer not found." });
        }

        var applications = await _db.Applications
            .Where(a => a.VolunteerId == volunteerId)
            .Include(a => a.Event)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new
            {
                a.Id,
                a.EventId,
                a.VolunteerId,
                a.Status,
                a.AppliedAt,
                Event = new
                {
                    a.Event.Id,
                    a.Event.Title,
                    a.Event.Location,
                    a.Event.City,
                    a.Event.EventDate,
                    a.Event.ShiftStart,
                    a.Event.ShiftEnd,
                    a.Event.PayPerDay,
                    a.Event.Description,
                    a.Event.Status
                }
            })
            .ToListAsync();

        return Ok(applications);
    }
}