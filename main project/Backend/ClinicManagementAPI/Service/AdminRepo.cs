using ClinicManagementAPI.Context;
using ClinicManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.Service
{
    public class AdminRepo : IAdminRepo
    {
        private readonly PasswordHasher<User> passwordhash;
        private readonly AppDbContext context;

        public AdminRepo(AppDbContext dbContext)
        {
            context = dbContext;
            passwordhash = new PasswordHasher<User>();
        }

        #region Add Doctor & Receptionist
        public void AddDoctor(DoctorDto doctor)
        {
            var role = context.Roles.FirstOrDefault(r => r.RoleName.ToLower() == doctor.RoleName.ToLower());

            if (role == null)
                throw new Exception($"Role '{doctor.RoleName}' does not exist.");

            var existingUser = context.Users.FirstOrDefault(u => u.Username == doctor.userName);

            User userEntity;
            if (existingUser == null)
            {
                userEntity = new User
                {
                    Username = doctor.userName,
                    RoleId = role.RoleId,
                    Email = doctor.email,
                    PhoneNumber = doctor.phoneNumber,
                    Password = passwordhash.HashPassword(null, doctor.password)
                };

                context.Users.Add(userEntity);
                context.SaveChanges();
            }
            else
            {
                userEntity = existingUser;
            }

            var doctorEntity = new Doctor
            {
                DoctorName = doctor.doctorName,
                Age = doctor.age,
                Specialization = doctor.specialization,
                Email = doctor.email,
                PhoneNumber = doctor.phoneNumber,
                ConsultantFee = doctor.consultantFee,
                UserID = userEntity.UserId
            };

            context.Doctors.Add(doctorEntity);
            context.SaveChanges();
        }

        public void AddReception(ReceptionistDT receptionist)
        {
            var role = context.Roles.FirstOrDefault(r => r.RoleName.ToLower() == receptionist.RoleName.ToLower());

            if (role == null)
                throw new Exception($"Role '{receptionist.RoleName}' does not exist.");

            var existingUser = context.Users.FirstOrDefault(u => u.Username == receptionist.UserName);

            User userEntity;
            if (existingUser == null)
            {
                userEntity = new User
                {
                    Username = receptionist.UserName,
                    RoleId = role.RoleId,
                    Email = receptionist.Email,
                    PhoneNumber = receptionist.PhoneNumber,
                    Password = passwordhash.HashPassword(null, receptionist.Password)
                };

                context.Users.Add(userEntity);
                context.SaveChanges();
            }
            else
            {
                userEntity = existingUser;
            }

            var receptionistEntity = new Receptionist
            {
                ReceptionistName = receptionist.ReceptionistName,
                PhoneNumber = receptionist.PhoneNumber,
                Email = receptionist.Email,
                UserID = userEntity.UserId
            };

            context.Receptionists.Add(receptionistEntity);
            context.SaveChanges();
        }
        #endregion

        #region Get All
        public IEnumerable<DoctorResponseDto> GetAllDoctors()
        {
            return context.Doctors.Select(d => new DoctorResponseDto
            {
                DoctorID = d.DoctorID,
                DoctorName = d.DoctorName,
                Age = d.Age,
                Specialization = d.Specialization,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                ConsultantFee = d.ConsultantFee
            }).ToList();
        }

        public IEnumerable<GetRecep> GetAllReceptionist()
        {
            return context.Receptionists.Select(r => new GetRecep
            {
                ReceptionistID = r.ReceptionistID,
                ReceptionistName = r.ReceptionistName,
                PhoneNumber = r.PhoneNumber,
                Email = r.Email
            }).ToList();
        }
        #endregion

        #region Update Doctor
        public void UpdateDoctor(int doctorId, DoctorDto doctor)
        {
            var existingDoctor = context.Doctors.FirstOrDefault(d => d.DoctorID == doctorId);

            if (existingDoctor == null)
                throw new Exception("Doctor not found.");

            existingDoctor.DoctorName = doctor.doctorName;
            existingDoctor.Age = doctor.age;
            existingDoctor.Specialization = doctor.specialization;
            existingDoctor.Email = doctor.email;
            existingDoctor.PhoneNumber = doctor.phoneNumber;
            existingDoctor.ConsultantFee = doctor.consultantFee;

            context.SaveChanges();
        }
        #endregion

        #region Delete Doctor
        public void DeleteDoctor(int doctorId)
        {
            var doctor = context.Doctors.FirstOrDefault(d => d.DoctorID == doctorId);

            if (doctor == null)
                throw new Exception("Doctor not found.");
            var user = context.Users.FirstOrDefault(u => u.UserId == doctor.UserID);
            if (user != null)
            {
                context.Users.Remove(user);
            }

            context.Doctors.Remove(doctor);
            context.SaveChanges();
        }
        #endregion

        #region Update Receptionist
        public void UpdateReceptionist(int receptionistId, ReceptionistDT receptionist)
        {
            var existingReceptionist = context.Receptionists.FirstOrDefault(r => r.ReceptionistID == receptionistId);

            if (existingReceptionist == null)
                throw new Exception("Receptionist not found.");

            existingReceptionist.ReceptionistName = receptionist.ReceptionistName;
            existingReceptionist.Email = receptionist.Email;
            existingReceptionist.PhoneNumber = receptionist.PhoneNumber;

            context.SaveChanges();
        }
        #endregion

        #region Delete Receptionist
        public void DeleteReceptionist(int receptionistId)
        {
            var receptionist = context.Receptionists.FirstOrDefault(r => r.ReceptionistID == receptionistId);

            if (receptionist == null)
                throw new Exception("Receptionist not found.");

            var user = context.Users.FirstOrDefault(u => u.UserId == receptionist.UserID);
            if (user != null)
            {
                context.Users.Remove(user);
            }

            context.Receptionists.Remove(receptionist);
            context.SaveChanges();
        }
        #endregion
    }
}
