using JobASP.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace JobASP.Controllers
{
    [Authorize] // Bảo vệ toàn bộ controller, yêu cầu xác thực cho tất cả các hành động
    public class StaffController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StaffController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private void AddAuthorizationHeader(HttpClient client)
        {
            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

 
        [HttpGet]
        public async Task<IActionResult> Staff()
        {
            var client = _httpClientFactory.CreateClient();
            AddAuthorizationHeader(client);
            var response = await client.GetAsync("https://localhost:7264/api/Staff/get-all-staff");

            if (response.IsSuccessStatusCode)
            {
                var staffList = await response.Content.ReadFromJsonAsync<IEnumerable<StaffDTO>>();
                return View(staffList);
            }
            else
            {
                ViewBag.Error = "Không thể tải danh sách nhân viên.";
                return View("Error");
            }
        }


        [HttpGet]
  
        public IActionResult CreateStaff()
        {
            return View();
        }


        [HttpPost]
   
        public async Task<IActionResult> CreateStaff([FromForm] AddStaffRequestDTO staffRequestDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    var httpRequestMess = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri("https://localhost:7264/api/Staff/add-staff"),
                        Content = new StringContent(JsonSerializer.Serialize(staffRequestDTO), Encoding.UTF8, MediaTypeNames.Application.Json)
                    };
                    var httpResponseMess = await client.SendAsync(httpRequestMess);
                    httpResponseMess.EnsureSuccessStatusCode();

                    return RedirectToAction("Staff");
                }
                else
                {
                    return View(staffRequestDTO);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }


        [HttpGet]
        public async Task<IActionResult> DetailStaff(int id)
        {
            StaffDTO response = new StaffDTO();
            try
            {
                var client = _httpClientFactory.CreateClient();
                AddAuthorizationHeader(client);
                var httpResponseMess = await client.GetAsync("https://localhost:7264/api/Staff/get-staff-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                response = await httpResponseMess.Content.ReadFromJsonAsync<StaffDTO>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        
        [HttpGet]
        public async Task<IActionResult> EditStaff(int id)
        {
            StaffDTO responseStaff = null;
            var client = _httpClientFactory.CreateClient();
            AddAuthorizationHeader(client);
            try
            {
                var httpResponseMess = await client.GetAsync($"https://localhost:7264/api/Staff/get-staff-by-id/{id}");

                if (httpResponseMess.IsSuccessStatusCode)
                {
                    responseStaff = await httpResponseMess.Content.ReadFromJsonAsync<StaffDTO>();
                }
                else
                {
                    ViewBag.Error = "Nhân viên không tồn tại hoặc có lỗi trong quá trình lấy dữ liệu.";
                    return View("Error");
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                ViewBag.Error = $"Lỗi khi gửi yêu cầu: {httpRequestException.Message}";
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Đã xảy ra lỗi: {ex.Message}";
                return View("Error");
            }

            return View(responseStaff);
        }

    
        [HttpPost]
        public async Task<IActionResult> EditStaff(int id, [FromForm] StaffDTO staff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = _httpClientFactory.CreateClient();
                    AddAuthorizationHeader(client);
                    var httpRequestMess = new HttpRequestMessage()
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri($"https://localhost:7264/api/Staff/update-staff-by-id/{id}"),
                        Content = new StringContent(JsonSerializer.Serialize(staff), Encoding.UTF8, MediaTypeNames.Application.Json)
                    };
                    var httpResponseMess = await client.SendAsync(httpRequestMess);
                    httpResponseMess.EnsureSuccessStatusCode();

                    var updatedStaff = await httpResponseMess.Content.ReadFromJsonAsync<StaffDTO>();
                    if (updatedStaff != null)
                    {
                        return RedirectToAction("Staff");
                    }
                    else
                    {
                        ViewBag.Error = "Không thể cập nhật nhân viên.";
                        return View("Error");
                    }
                }
                else
                {
                    return View(staff);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        
        [HttpDelete]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid staff ID.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                AddAuthorizationHeader(client);
                var httpResponse = await client.DeleteAsync($"https://localhost:7264/api/Staff/delete-staff-by-id/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    return Ok(new { message = "Staff deleted successfully" });
                }
                else
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to delete staff. Status code: {httpResponse.StatusCode}, Reason: {httpResponse.ReasonPhrase}, Response: {responseContent}");
                    return StatusCode((int)httpResponse.StatusCode, "Failed to delete staff. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the staff.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchStaff(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var client = _httpClientFactory.CreateClient();
            AddAuthorizationHeader(client);
            var response = await client.GetAsync("https://localhost:7264/api/Staff/get-all-staff");

            if (response.IsSuccessStatusCode)
            {
                var staffList = await response.Content.ReadFromJsonAsync<IEnumerable<StaffDTO>>();
                if (!string.IsNullOrEmpty(searchString))
                {
                    staffList = staffList.Where(s =>
                        s.StaffID.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        s.StaffName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        s.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        s.Position.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }
                return View("Staff", staffList);
            }
            else
            {
                ViewBag.Error = "Không thể tải danh sách nhân viên.";
                return View("Error");
            }
        }

    }
}
