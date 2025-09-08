using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementAPI.Models;

public class DoctorAvailability
    {
        [Key]
       public int AvailabilityId { get; set; }
    public string Day { get; set; } 
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    }