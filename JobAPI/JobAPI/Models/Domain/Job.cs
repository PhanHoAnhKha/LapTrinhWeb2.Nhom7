using System.ComponentModel.DataAnnotations;

namespace JobAPI.Models.Domain
{
    public class Job
    {
        [Key]
        public int JobID { get; set; }
        public string? Title { get; set; } // Tiêu đề của công việc
        public string? Description { get; set; } // Mô tả chi tiết về công việc
        public DateTime StartDate { get; set; } // Ngày giao công việc
        public string? Status { get; set; } // Tiến trình
        public List<JobStaff> JobStaffs { get; set; }
    }
}
