using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicManagementAPI.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }

        [Required]
        public string? DoctorName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string? Specialization { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int ConsultantFee { get; set; }
        public int isDeleted { get; set; } = 0;

        [ForeignKey("UserID")]
        public int UserID { get; set; }

        public User? User { get; set; }
        [JsonIgnore]
        public ICollection<DoctorAvailability> Availabilities { get; set; } = new List<DoctorAvailability>();
        [JsonIgnore]
       public ICollection<Appointment>? Appointments { get; set; }
        
    }
}