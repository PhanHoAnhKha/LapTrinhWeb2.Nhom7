
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAPI.Data;
using JobAPI.Models.Domain;
using JobAPI.Models.DTO;
using JobAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Jobs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly AppDbContext _appContext;
        private readonly IJobRepository _jobRepository;

        public JobController(AppDbContext dbContext, IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
            _appContext = dbContext;
        }


        [HttpGet("get-all-jobs")]
        [Authorize(Roles = "Read,Write")]
        public IActionResult GetAllJob()
        {
            try
            {
                var allJobs = _jobRepository.GetAllJob();
                if (allJobs.Count == 0)
                {
                    return NotFound();
                }
                return Ok(allJobs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UpdateStatus(int id)
        {
            var job = _appContext.Jobs.FirstOrDefault(j => j.JobID == id);
            if (job != null)
            {
                job.Status = "Done";
                _appContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("get-job-by-id/{id}")]
        [Authorize(Roles = "Read,Write")]
        public IActionResult GetJobById([FromRoute] int id)
        {
            try
            {
                var jobDTO = _jobRepository.GetJobById(id);
                if (jobDTO == null)
                {
                    return NotFound();
                }
                return Ok(jobDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-job")]
        [Authorize(Roles = "Write")]
        public IActionResult AddJob([FromBody] AddJobRequestDTO addJobRequestDTO)
        {
            try
            {
                var jobAdd = _jobRepository.AddJob(addJobRequestDTO);
                return Ok(jobAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("update-job-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult UpdateJobById(int id, [FromBody] AddJobRequestDTO jobDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedJob = _jobRepository.UpdateJobById(id, jobDTO);
                return Ok(updatedJob);
            }
            catch (Exception ex)
            {
                var errorDetails = new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                };

                return BadRequest(errorDetails);
            }
        }



        [HttpDelete("delete-job-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult DeleteJobById(int id)
        {
            try
            {
                var deletedJob = _jobRepository.DeleteJobById(id);
                if (deletedJob == null)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
