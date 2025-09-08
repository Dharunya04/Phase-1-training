using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Receptionist
    {
        [Key]
        public int ReceptionistID { get; set; }

        [Required]
        public string ReceptionistName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int isDeleted { get; set; } = 0;

        [ForeignKey("UserID")]
        public int UserID { get; set; }
        public User? User { get; set; }
    }