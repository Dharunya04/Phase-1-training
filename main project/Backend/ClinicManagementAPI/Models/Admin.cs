using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        [Required]
        public string? AdminName { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Email { get; set; }

        public int isDeleted { get; set; } = 0;

        [ForeignKey("UserID")]
        public int UserID { get; set; }

        public User? User { get; set; }
    }