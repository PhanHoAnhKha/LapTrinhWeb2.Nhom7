using JobAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace JobAPI.Data
{
    public class SQL
    {
        private readonly ModelBuilder _builder;

        public SQL(ModelBuilder builder)
        {
            _builder = builder;
        }

        public void Seed()
        {
            _builder.Entity<Staff>().HasData(
                new Staff
                {
                    StaffID = 1,
                    StaffName = "Nguyễn Hữu Thạch",
                    Email = "thach04@gmail.com",
                    Position = "Hậu Cần",
                },
                new Staff
                {
                    StaffID = 2,
                    StaffName = "Trần Kim Long",
                    Email = "long04@gmail.com",
                    Position = "Trưởng Phòng",
                }
            );


            _builder.Entity<Job>().HasData(
                new Job
                {
                    JobID = 1,
                    Title = "Designer",
                    Description = "Thiết kế những fontend đẹp và hoành tráng.",
                    StartDate = new DateTime(2023, 6, 1),
                    Status = "Mới làm",
                },
                new Job
                {
                    JobID = 2,
                    Title = "IT",
                    Description = "Làm ra những trang web hiện tại.",
                    StartDate = new DateTime(2023, 7, 1),
                    Status = "Sắp xong",
                }
            );
        }
    }
}

