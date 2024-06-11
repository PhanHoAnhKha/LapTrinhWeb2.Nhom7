using JobAPI.Models.Domain;
using JobAPI.Models.DTO;

namespace JobAPI.Repositories
{
    public interface IStaffRepository
    {
        List<StaffDTO> GetAllStaff();
        StaffDTO GetStaffById(int id);
        AddStaffRequestDTO AddStaff(AddStaffRequestDTO addStaffRequestDTO);
        AddStaffRequestDTO? UpdateStaffById(int id, AddStaffRequestDTO addStaffRequestDTO);
        Staff? DeleteStaffById(int id);
    }
}

