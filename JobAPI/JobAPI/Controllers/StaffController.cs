
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using JobAPI.Data;
using JobAPI.Models.DTO;
using JobAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class StaffController : ControllerBase
    {
        private readonly AppDbContext _appContext;
        private readonly IStaffRepository _staffRepository;

        public StaffController(AppDbContext dbContext, IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
            _appContext = dbContext;
        }

       
        [HttpGet("get-all-staff")]
        [Authorize(Roles = "Read,Write")]
        public IActionResult GetAllStaff()
        {
            try
            {
                var allStaff = _staffRepository.GetAllStaff();
                if (allStaff.Count == 0)
                {
                    return NotFound();
                }
                return Ok(allStaff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-staff-by-id/{id}")]
        [Authorize(Roles = "Read,Write")]
        public IActionResult GetStaffById(int id)
        {
            try
            {
                var staffDTO = _staffRepository.GetStaffById(id);
                if (staffDTO == null)
                {
                    return NotFound();
                }
                return Ok(staffDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-staff")]
        [Authorize(Roles = "Write")]
        public IActionResult AddStaff([FromBody] AddStaffRequestDTO addStaffRequestDTO)
        {
            try
            {
                // Kiểm tra vai trò của người dùng
                var userRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

                // Nếu người dùng chỉ có vai trò "Read", trả về thông báo lỗi chi tiết
                if (userRoles.Contains("Read") && !userRoles.Contains("Write"))
                {
                    return Forbid("Bạn không có quyền sửa. Vui lòng liên hệ quản trị viên để được cấp quyền.");
                }

                // Thực hiện hành động thêm nhân viên
                var addedStaff = _staffRepository.AddStaff(addStaffRequestDTO);
                return Ok(addedStaff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("update-staff-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult UpdateStaffById(int id, [FromBody] AddStaffRequestDTO staffDTO)
        {
            try
            {
                var existingStaff = _staffRepository.GetStaffById(id);
                if (existingStaff == null)
                {
                    return NotFound();
                }

                var updatedStaff = _staffRepository.UpdateStaffById(id, staffDTO);
                return Ok(updatedStaff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpDelete("delete-staff-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult DeleteStaffById(int id)
        {
            try
            {
                var deletedStaff = _staffRepository.DeleteStaffById(id);
                if (deletedStaff == null)
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

