using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Billing
    {
        [Key]
        public int InvoiceID { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        
        [ForeignKey("PatientID")]
        public int PatientID { get; set; }

        [ForeignKey("AppointmentID")]
        public int AppointmentID { get; set; }

        
        public Patient? Patient { get; set; }
        public Appointment? Appointment { get; set; }

    }