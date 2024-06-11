using JobAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace JobAPI.Data
{
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {

            }
            public DbSet<Job> Jobs { get; set; }
            public DbSet<Staff> Staffs { get; set; }
            public DbSet<JobStaff> JobStaffs { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                builder.Entity<JobStaff>().HasOne(js => js.Job).WithMany(j => j.JobStaffs).HasForeignKey(js => js.JobID);

                builder.Entity<JobStaff>().HasOne(js => js.Staff).WithMany(s => s.JobStaffs).HasForeignKey(js => js.StaffID);

                // Gọi phương thức Seed để thêm dữ liệu mẫu
                new SQL(builder).Seed();
            }
        }
    }

