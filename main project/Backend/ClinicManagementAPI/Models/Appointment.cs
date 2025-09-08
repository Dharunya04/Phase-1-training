using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementAPI.Models;
    public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? AppointmentStatus { get; set; }
    public string? ReasonForVisit { get; set; }

    public int PatientID { get; set; }
    public int DoctorID { get; set; }

    [ForeignKey("DoctorID")]
    public Doctor? Doctor { get; set; }

    [ForeignKey("PatientID")]
    public Patient? Patient { get; set; }
}
