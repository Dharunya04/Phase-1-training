using ClinicManagementAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Service;

public class ReceptionRepo : IReceptionRepo
{
    private readonly AppDbContext _context;

    public ReceptionRepo(AppDbContext context)
    {
        _context = context;
    }

    public void AddPatient(Patient patient)
    {
        _context.Patients.Add(patient);
        _context.SaveChanges();
    }

    public IEnumerable<Patient> GetAllPatients()
    {
        return _context.Patients.Where(p => p.isDeleted == 0).ToList();
    }
    public Appointment AutoScheduleAppointment(int patientId, int doctorId, string? reasonForVisit)
    {
        var doctor = _context.Doctors
            .Include(d => d.Availabilities)
            .Include(d => d.Appointments)
            .FirstOrDefault(d => d.DoctorID == doctorId);

        if (doctor == null)
            throw new Exception("Doctor not found");

        foreach (var availability in doctor.Availabilities.OrderBy(a => a.Date).ThenBy(a => a.StartTime))
        {
            DateTime slotTime = availability.Date.Date + availability.StartTime;

            bool isBooked = doctor.Appointments
                .Any(a => a.AppointmentDate == slotTime);

            if (!isBooked)
            {
                var appointment = new Appointment
                {
                    PatientID = patientId,
                    DoctorID = doctorId,
                    AppointmentDate = slotTime,
                    AppointmentStatus = "Scheduled",
                    ReasonForVisit = reasonForVisit
                };

                _context.Appointments.Add(appointment);
                _context.SaveChanges();
                return appointment;
            }
        }

        throw new Exception("No available slots for this doctor.");
    }

    public IEnumerable<Appointment> GetAppointments()
    {
        return _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ToList();
    }

    

    
}