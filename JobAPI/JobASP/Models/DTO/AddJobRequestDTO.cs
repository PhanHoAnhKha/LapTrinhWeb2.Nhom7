namespace JobASP.Models.DTO
{
    public class AddJobRequestDTO
    {
        public int JobID { get; set; }
        public string? Title { get; set; } // Tiêu đề của công việc
        public string? Description { get; set; } // Mô tả chi tiết về công việc
        public DateTime StartDate { get; set; } // Ngày giao công việc
        public string? Status { get; set; } // Trạng thái công việc
        public List<int> StaffID { get; set; }

    }
}
