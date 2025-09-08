using ClinicManagementAPI.Models;

namespace ClinicManagementAPI.Service
{
    public interface IAdminRepo
    {
        void AddDoctor(DoctorDto doctor);
        void AddReception(ReceptionistDT receptionist);

        IEnumerable<DoctorResponseDto> GetAllDoctors();
        IEnumerable<GetRecep> GetAllReceptionist();

        void UpdateDoctor(int doctorId, DoctorDto doctor);
        void DeleteDoctor(int doctorId);

        void UpdateReceptionist(int receptionistId, ReceptionistDT receptionist);
        void DeleteReceptionist(int receptionistId);
    }
}
