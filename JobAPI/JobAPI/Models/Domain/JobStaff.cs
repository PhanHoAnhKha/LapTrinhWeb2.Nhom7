namespace JobAPI.Models.Domain
{
    public class JobStaff
    {
        public int ID { get; set; }

        public int JobID { get; set; }
        public Job? Job { get; set; }
        public int StaffID { get; set; }
        public Staff? Staff { get; set; }
    }
}
