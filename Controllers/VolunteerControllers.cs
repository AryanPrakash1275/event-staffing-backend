using EventStaffing.Api.Data;
using EventStaffing.Api.DTOs;
using EventStaffing.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventStaffing.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VolunteersController : ControllerBase
{
    private readonly AppDbContext _db;

    public VolunteersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] VolunteerLoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Phone))
        {
            return BadRequest(new { message = "Phone is required." });
        }

        var phone = request.Phone.Trim();

        var existingVolunteer = await _db.Volunteers
            .FirstOrDefaultAsync(v => v.Phone == phone);

        if (existingVolunteer is not null)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                existingVolunteer.Name = request.Name.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.City))
            {
                existingVolunteer.City = request.City.Trim();
            }

            await _db.SaveChangesAsync();

            return Ok(existingVolunteer);
        }

        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.City))
        {
            return BadRequest(new { message = "Name and city are required for first-time login." });
        }

        var volunteer = new Volunteer
        {
            Phone = phone,
            Name = request.Name.Trim(),
            City = request.City.Trim()
        };

        _db.Volunteers.Add(volunteer);
        await _db.SaveChangesAsync();

        return Ok(volunteer);
    }
}