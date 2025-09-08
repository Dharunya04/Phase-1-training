using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementAPI.Models;

public class MedicalRecord
    {
        [Key]
        public int MedicalRecordID { get; set; }

        public string? MedicalNotes { get; set; }

        [Required]
        public int IsBillGenerated { get; set; } = 0;

        // Foreign key relationships
        [ForeignKey("PatientID")]
        public int PatientID { get; set; }

        [ForeignKey("AppointmentID")]
        public int AppointmentID { get; set; }

        [ForeignKey("DoctorID")]
        public int DoctorID { get; set; }

        public Patient? Patient { get; set; }
        public Appointment? Appointment { get; set; }
        public Doctor? Doctor { get; set; }
    }