using Microsoft.AspNetCore.Mvc;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Service;

[ApiController]
[Route("api/[controller]")]
public class ReceptionController : ControllerBase
{
    private readonly IReceptionRepo _receptionRepo;

    public ReceptionController(IReceptionRepo ReceptionRepo)
    {
        _receptionRepo = ReceptionRepo;
    }
    [HttpPost("AddPatient")]
    public IActionResult AddPatient([FromBody] Patient patient)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _receptionRepo.AddPatient(patient);
        return Ok(new { message = "Patient added successfully!" });
    }

    [HttpGet("GetAllPatients")]
    public IActionResult GetAllPatients()
    {
        var patients = _receptionRepo.GetAllPatients();
        return Ok(patients);
    }
    

   [HttpPost("AutoSchedule")]
public IActionResult AutoSchedule([FromBody] AutoScheduleRequest request)
{
    try
    {
        var appointment = _receptionRepo
            .AutoScheduleAppointment(request.PatientID, request.DoctorID, request.ReasonForVisit);

        return Ok(new { message = "Appointment scheduled successfully!" });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
   [HttpGet("GetAppointments")]
public IActionResult GetAppointments()
{
    var appointments = _receptionRepo.GetAppointments()
        .Select(a => new AppointmentDto
        {
            AppointmentId = a.AppointmentId,
            AppointmentDate = a.AppointmentDate,
            AppointmentStatus = a.AppointmentStatus,
            ReasonForVisit = a.ReasonForVisit,
            DoctorName = a.Doctor != null ? a.Doctor.DoctorName : "",
            PatientName = a.Patient != null ? a.Patient.FirstName + " " + a.Patient.LastName : ""
        })
        .ToList();

    return Ok(appointments);
}

}
