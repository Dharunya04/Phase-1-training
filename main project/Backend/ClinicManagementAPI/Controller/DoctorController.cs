using ClinicManagementAPI.Models;
using ClinicManagementAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicManagementAPI.DTOs;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]

public class DoctorController : ControllerBase
{
    private readonly IDoctorRepo _doctorRepo;

    public DoctorController(IDoctorRepo doctorRepo)
    {
        _doctorRepo = doctorRepo;
    }

    [HttpGet("GetLoggedDoctor")]
    [Authorize(Roles = "Doctor")]
    public IActionResult GetLoggedDoctor()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized("Invalid token.");

        var doctor = _doctorRepo.GetDoctorDtoById(int.Parse(userId));
        if (doctor == null)
            return NotFound("Doctor not found.");

        return Ok(doctor);
    }
    [HttpPost("availability")]
    [Authorize(Roles = "Doctor")]
    public IActionResult AddAvailability(AvailabilityDto dto)
    {

      // ✅ Get doctorId from JWT token
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userIdClaim == null)
        return Unauthorized("You can only add availability for your account.");

    if (!int.TryParse(userIdClaim, out int doctorId))
        return BadRequest("Invalid doctor ID in token.");

    // ✅ Check doctor exists
    var doctor = _doctorRepo.GetDoctorById(doctorId);
    if (doctor == null)
        return NotFound("Doctor not found.");

    // ✅ Parse Date & Time safely
    if (!DateTime.TryParse(dto.Date, out DateTime date))
        return BadRequest("Invalid date format.");
    if (!TimeSpan.TryParse(dto.StartTime, out TimeSpan startTime))
        return BadRequest("Invalid start time format.");
    if (!TimeSpan.TryParse(dto.EndTime, out TimeSpan endTime))
        return BadRequest("Invalid end time format.");

    // ✅ Check if availability already exists
    if (_doctorRepo.AvailabilityExists(doctorId, date, startTime, endTime))
    {
        return Conflict(new { message = "This availability slot already exists!" });
    }

    // ✅ Create new availability
    var availability = new DoctorAvailability
    {
        Day = dto.Day,
        Date = date,
        StartTime = startTime,
        EndTime = endTime,
        DoctorId = doctorId
    };

    // ✅ Save in DB
    _doctorRepo.AddAvailability(availability);
    _doctorRepo.Save();

    return Ok(new { message = "Availability added successfully." });
    }

    [HttpGet("GetAllAvailabilities")]
    public IActionResult GetAllAvailabilities()
    {
        var response = _doctorRepo.GetAllAvailabilities();
        return Ok(response);
    }

    [HttpPut("UpdateAvailability/{id}")]
    public IActionResult UpdateAvailability(int id, AvailabilityDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        if (!int.TryParse(userIdClaim, out int doctorId)) return BadRequest("Invalid doctor ID");

        var doctor = _doctorRepo.GetDoctorById(doctorId);
        if (doctor == null) return NotFound("Doctor not found");

        var updated = new DoctorAvailability
        {
            Day = dto.Day,
            Date = DateTime.Parse(dto.Date),
            StartTime = TimeSpan.Parse(dto.StartTime),
            EndTime = TimeSpan.Parse(dto.EndTime),
            DoctorId = doctorId
        };

        try
        {
            _doctorRepo.UpdateAvailability(id, updated);
            return Ok(new { message = "Availability updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteAvailability/{id}")]
    public IActionResult DeleteAvailability(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        if (!int.TryParse(userIdClaim, out int doctorId)) return BadRequest("Invalid doctor ID");

        try
        {
            _doctorRepo.DeleteAvailability(id);
            return Ok(new { message = "Availability deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("my-appointments")]
    public IActionResult GetMyAppointments()
    {
        // ✅ Get doctorId from JWT token
        var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (doctorIdClaim == null)
            return Unauthorized("Doctor not logged in");

        int doctorId = int.Parse(doctorIdClaim.Value);


        var appointments = _doctorRepo.GetAppointmentsForDoctor(doctorId);

        if (appointments == null || !appointments.Any())
            return NotFound("No appointments found");

        return Ok(appointments);
    }
    
    [HttpDelete("complete-appointment/{appointmentId}")]
public IActionResult CompleteAppointment(int appointmentId)
{
    var success = _doctorRepo.CompleteAppointment(appointmentId);

    if (!success)
        return NotFound(new { message = "Appointment not found" });

    return Ok(new { message = "Appointment marked as completed and removed" });
}

}



