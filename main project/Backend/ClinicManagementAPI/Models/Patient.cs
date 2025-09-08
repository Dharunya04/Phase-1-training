using System.ComponentModel.DataAnnotations;

public class Patient
{
    [Key]
    public int PatientID { get; set; }

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public int isDeleted { get; set; } = 0;
        
    public ICollection<Appointment>? Appointments { get; set; }

    }