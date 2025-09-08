using ClinicManagementAPI.Models;

public interface IDoctorRepo
{
    Doctor? GetDoctorById(int doctorId);
    DoctorDto? GetDoctorDtoById(int doctorId);
    void AddAvailability(DoctorAvailability availability);
    IEnumerable<AvailabilityResponseDto> GetAllAvailabilities();
    bool AvailabilityExists(int doctorId, DateTime date, TimeSpan startTime, TimeSpan endTime);
    void Save();
    void UpdateAvailability(int availabilityId, DoctorAvailability updated);
    void DeleteAvailability(int availabilityId);
    IEnumerable<object> GetAppointmentsForDoctor(int doctorId);
    bool CompleteAppointment(int appointmentId);
}
