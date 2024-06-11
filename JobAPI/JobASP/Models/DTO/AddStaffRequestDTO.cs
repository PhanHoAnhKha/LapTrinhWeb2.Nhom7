namespace JobASP.Models.DTO
{
    public class AddStaffRequestDTO
    {
        public int StaffID { get; set; }
        public string? StaffName { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
    }

}
