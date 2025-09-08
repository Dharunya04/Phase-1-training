using ClinicManagementAPI.Models;
using ClinicManagementAPI.Service;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminRepo adminRepo;

    public AdminController(IAdminRepo repo)
    {
        adminRepo = repo;
    }

    [HttpPost("Doctor")]
    public IActionResult AddDoctor([FromBody] DoctorDto doctor)
    {
        adminRepo.AddDoctor(doctor);
        return Ok(new { message = "Doctor added successfully" });
    }

    [HttpPost("Reception")]
    public IActionResult AddReception([FromBody] ReceptionistDT receptionist)
    {
        adminRepo.AddReception(receptionist);
        return Ok(new { message = "Receptionist added successfully" });
    }

    [HttpGet("GetAllDoctor")]
    public IActionResult GetAllDoctor()
    {
        var doctors = adminRepo.GetAllDoctors();
        return Ok(doctors);
    }

    [HttpGet("GetAllReceptionist")]
    public IActionResult GetAllReceptionist()
    {
        var receptionists = adminRepo.GetAllReceptionist();
        return Ok(receptionists);
    }

    [HttpPut("UpdateDoctor/{id}")]
    public IActionResult UpdateDoctor(int id, [FromBody] DoctorDto doctor)
    {
        try
        {
            adminRepo.UpdateDoctor(id, doctor);
            return Ok(new { message = "Doctor updated successfully" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("DeleteDoctor/{id}")]
    public IActionResult DeleteDoctor(int id)
    {
        try
        {
            adminRepo.DeleteDoctor(id);
            return Ok(new { message = "Doctor deleted successfully" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("UpdateReceptionist/{id}")]
    public IActionResult UpdateReceptionist(int id, [FromBody] ReceptionistDT receptionist)
    {
        try
        {
            adminRepo.UpdateReceptionist(id, receptionist);
            return Ok(new { message = "Receptionist updated successfully" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("DeleteReceptionist/{id}")]
    public IActionResult DeleteReceptionist(int id)
    {
        try
        {
            adminRepo.DeleteReceptionist(id);
            return Ok(new { message = "Receptionist deleted successfully" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
