using ClinicManagementAPI.Context;
using ClinicManagementAPI.DTOs;
using ClinicManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClinicManagementAPI.Service
{
    public class DoctorRepo : IDoctorRepo
    {
        private readonly AppDbContext _context;

        public DoctorRepo(AppDbContext context)
        {
            _context = context;
        }

        public Doctor? GetDoctorById(int doctorId)
        {
            return _context.Doctors.FirstOrDefault(d => d.DoctorID == doctorId);
        }

        public DoctorDto? GetDoctorDtoById(int doctorId)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.DoctorID == doctorId);
            if (doctor == null) return null;

            return new DoctorDto
            {
                doctorName = doctor.DoctorName,
                specialization = doctor.Specialization,
                age = doctor.Age,
                email = doctor.Email,
                phoneNumber = doctor.PhoneNumber,
                consultantFee = doctor.ConsultantFee
            };
        }

        public void AddAvailability(DoctorAvailability availability)
        {
            _context.availabilities.Add(availability);
        }
        public bool AvailabilityExists(int doctorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
{
    return _context.availabilities.Any(a =>
        a.DoctorId == doctorId &&
        a.Date.Date == date.Date &&
        a.StartTime == startTime &&
        a.EndTime == endTime
    );
}

        public IEnumerable<AvailabilityResponseDto> GetAllAvailabilities()
        {
            var availabilities = _context.availabilities
                .Include(d => d.Doctor)
                .ToList();

            var appointments = _context.Appointments
                .Where(a => a.AppointmentStatus == "Scheduled")
                .ToList();

            var response = availabilities.Select(a => new AvailabilityResponseDto
            {
                AvailabilityId = a.AvailabilityId,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor.DoctorName,
                Day = a.Day,
                Date = a.Date,
                StartTime = a.StartTime.ToString(@"hh\:mm"),
                EndTime = a.EndTime.ToString(@"hh\:mm"),

                IsBooked = appointments.Any(appt =>
                    appt.DoctorID == a.DoctorId &&
                    a.Date != DateTime.MinValue &&
                    appt.AppointmentDate.Date == a.Date.Date &&
                    appt.AppointmentDate.TimeOfDay == a.StartTime
                )
            }).ToList();

            return response;
        }


        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateAvailability(int availabilityId, DoctorAvailability updated)
        {
            var existing = _context.availabilities.FirstOrDefault(a => a.AvailabilityId == availabilityId);
            if (existing == null)
                throw new Exception("Availability not found");

            existing.Day = updated.Day;
            existing.Date = updated.Date;
            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;

            _context.SaveChanges();
        }

        public void DeleteAvailability(int availabilityId)
        {
            var existing = _context.availabilities.FirstOrDefault(a => a.AvailabilityId == availabilityId);
            if (existing == null)
                throw new Exception("Availability not found");

            _context.availabilities.Remove(existing);
            _context.SaveChanges();
        }
        public IEnumerable<object> GetAppointmentsForDoctor(int doctorId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorID == doctorId && a.AppointmentStatus == "Scheduled")
                .Select(a => new
                {
                    a.AppointmentId,
                    PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                    a.AppointmentDate,
                    a.AppointmentStatus,
                    a.ReasonForVisit
                })
                .ToList();
        }


        public bool CompleteAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId== appointmentId);
            if (appointment == null)
                return false;

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return true;
        }
    }
}
