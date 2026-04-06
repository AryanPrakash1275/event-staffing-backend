using EventStaffing.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventStaffing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db;

    public EventsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _db.Events
            .Where(e => e.Status == "Open")
            .OrderBy(e => e.EventDate)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Location,
                e.City,
                e.EventDate,
                e.ShiftStart,
                e.ShiftEnd,
                e.PayPerDay,
                e.Description,
                e.RequiredVolunteers,
                e.Status,
                e.CreatedAt
            })
            .ToListAsync();

        var orderedEvents = events
            .OrderBy(e => e.EventDate)
            .ThenBy(e => e.ShiftStart)
            .ToList();

        return Ok(orderedEvents);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var eventItem = await _db.Events
            .Where(e => e.Id == id)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Location,
                e.City,
                e.EventDate,
                e.ShiftStart,
                e.ShiftEnd,
                e.PayPerDay,
                e.Description,
                e.RequiredVolunteers,
                e.Status,
                e.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (eventItem is null)
        {
            return NotFound(new { message = "Event not found." });
        }

        return Ok(eventItem);
    }
}