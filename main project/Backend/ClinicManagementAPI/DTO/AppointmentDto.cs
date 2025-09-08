public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string AppointmentStatus { get; set; }
    public string ReasonForVisit { get; set; }
    public string DoctorName { get; set; }
    public string PatientName { get; set; }
}


    public class DoctorAppointmentsDto
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientEmail { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    

