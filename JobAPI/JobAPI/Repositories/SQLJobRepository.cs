using JobAPI.Data;
using JobAPI.Models.Domain;
using JobAPI.Models.DTO;

namespace JobAPI.Repositories
{
    public class SQLJobRepository : IJobRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLJobRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<JobDTO> GetAllJob()
        {
            var allJobs = _dbContext.Jobs.Select(job => new JobDTO()
            {
                JobID = job.JobID,
                Title = job.Title,
                Description = job.Description,
                StartDate = job.StartDate,
                Status = job.Status,
                StaffName = job.JobStaffs.Select(n => n.Staff.StaffName).ToList(),
            }).ToList();

            return allJobs;
        }

        public JobDTO GetJobById(int id)
        {
            var job = _dbContext.Jobs.Where(j => j.JobID == id);

            var jobWithDomain = job.Select(job => new JobDTO()
            {
                JobID = job.JobID,
                Title = job.Title,
                Description = job.Description,
                StartDate = job.StartDate,
                Status = job.Status,
                StaffName = job.JobStaffs.Select(n => n.Staff.StaffName).ToList(),
            }).FirstOrDefault();

            return jobWithDomain;
        }

        public AddJobRequestDTO AddJob(AddJobRequestDTO addJobRequestDTO)
        {
            var job = new Job
            {
                Title = addJobRequestDTO.Title,
                Description = addJobRequestDTO.Description,
                StartDate = addJobRequestDTO.StartDate,
                Status = addJobRequestDTO.Status
            };

            _dbContext.Jobs.Add(job);
            _dbContext.SaveChanges();
            foreach (var id in addJobRequestDTO.StaffID)
            {
                var jobStaff = new JobStaff()
                {
                    JobID = job.JobID,
                    StaffID = id
                };
                _dbContext.JobStaffs.Add(jobStaff);
                _dbContext.SaveChanges();
            }
            return addJobRequestDTO;
        }


        public AddJobRequestDTO? UpdateJobById(int id, AddJobRequestDTO jobDTO)
        {
            var job = _dbContext.Jobs.FirstOrDefault(j => j.JobID == id);

            if (job != null)
            {
                job.Title = jobDTO.Title;
                job.Description = jobDTO.Description;
                job.StartDate = jobDTO.StartDate;
                job.Status = jobDTO.Status;

                // Cập nhật thông tin Job
                _dbContext.Jobs.Update(job);
                _dbContext.SaveChanges();

                // Xóa các bản ghi JobStaff hiện tại
                var existingJobStaffs = _dbContext.JobStaffs.Where(js => js.JobID == id).ToList();
                _dbContext.JobStaffs.RemoveRange(existingJobStaffs);
                _dbContext.SaveChanges();

                // Thêm các bản ghi JobStaff mới
                foreach (var staffId in jobDTO.StaffID)
                {
                    var jobStaff = new JobStaff()
                    {
                        JobID = job.JobID,
                        StaffID = staffId  // Đảm bảo sử dụng đúng StaffID
                    };
                    _dbContext.JobStaffs.Add(jobStaff);
                }
                _dbContext.SaveChanges();
            }

            return jobDTO;
        }




        public Job? DeleteJobById(int id)
        {
            var job = _dbContext.Jobs.FirstOrDefault(j => j.JobID == id);

            if (job != null)
            {
                _dbContext.Jobs.Remove(job);
                _dbContext.SaveChanges();
            }
            return job;
        }
    }
}
