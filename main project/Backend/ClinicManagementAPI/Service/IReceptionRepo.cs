namespace ClinicManagementAPI.Service
{
    public interface IReceptionRepo
    {
        void AddPatient(Patient patient);
        IEnumerable<Patient> GetAllPatients();

        Appointment AutoScheduleAppointment(int patientId, int doctorId, string? reasonForVisit);
        IEnumerable<Appointment> GetAppointments();
    
    
    }
}