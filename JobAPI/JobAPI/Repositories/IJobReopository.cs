using JobAPI.Models.Domain;
using JobAPI.Models.DTO;

namespace JobAPI.Repositories
{
    public interface IJobRepository
    {
        List<JobDTO> GetAllJob();
        JobDTO GetJobById(int id);
        AddJobRequestDTO AddJob(AddJobRequestDTO addBookRequestDTO);
        AddJobRequestDTO? UpdateJobById(int id, AddJobRequestDTO addBookRequestDTO);
        Job? DeleteJobById(int id);
    }
}

