using JobAPI.Data;
using JobAPI.Models.Domain;
using JobAPI.Models.DTO;

namespace JobAPI.Repositories
{
    public class SQLStaffRepository : IStaffRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLStaffRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<StaffDTO> GetAllStaff()
        {
            var allStaff = _dbContext.Staffs.Select(staff => new StaffDTO()
            {
                StaffID = staff.StaffID,
                StaffName = staff.StaffName,
                Email = staff.Email,
                Position = staff.Position
            }).ToList();

            return allStaff;
        }

        public StaffDTO GetStaffById(int id)
        {
            var staff = _dbContext.Staffs.Where(s => s.StaffID == id);

            var staffWithDomain = staff.Select(staff => new StaffDTO()
            {
                StaffID = staff.StaffID,
                StaffName = staff.StaffName,
                Email = staff.Email,
                Position = staff.Position
            }).FirstOrDefault();

            return staffWithDomain;
        }

        public AddStaffRequestDTO AddStaff(AddStaffRequestDTO addStaffRequestDTO)
        {
            var staff = new Staff
            {
                StaffName = addStaffRequestDTO.StaffName,
                Email = addStaffRequestDTO.Email,
                Position = addStaffRequestDTO.Position
            };

            _dbContext.Staffs.Add(staff);
            _dbContext.SaveChanges();

            return addStaffRequestDTO;
        }

        public AddStaffRequestDTO? UpdateStaffById(int id, AddStaffRequestDTO staffDTO)
        {
            var staff = _dbContext.Staffs.FirstOrDefault(s => s.StaffID == id);

            if (staff != null)
            {
                staff.StaffName = staffDTO.StaffName;
                staff.Email = staffDTO.Email;
                staff.Position = staffDTO.Position;

                _dbContext.SaveChanges();
            }

            return staffDTO;
        }

        public Staff? DeleteStaffById(int id)
        {
            var staff = _dbContext.Staffs.FirstOrDefault(s => s.StaffID == id);

            if (staff != null)
            {
                _dbContext.Staffs.Remove(staff);
                _dbContext.SaveChanges();
            }

            return staff;
        }
    }
}

