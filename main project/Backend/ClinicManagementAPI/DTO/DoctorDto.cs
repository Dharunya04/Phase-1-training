 public class DoctorDto
    {
        public string? userName { get; set; }
        public string? password { get; set; }
        public string? RoleName { get; set; }
        public string? doctorName { get; set; }
        public int age { get; set; }
        public string? specialization { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public int consultantFee { get; set; }
    }
public class DoctorResponseDto
{
    public int DoctorID { get; set; }
    public string? DoctorName { get; set; }
    public int Age { get; set; }
    public string? Specialization { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int ConsultantFee { get; set; }
}