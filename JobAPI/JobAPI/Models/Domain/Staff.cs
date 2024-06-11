using System.ComponentModel.DataAnnotations;

namespace JobAPI.Models.Domain
{
    public class Staff
    {
        [Key]
        public int StaffID { get; set; }
        public string? StaffName { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
        public List<JobStaff> JobStaffs { get; set; }
    }
}
