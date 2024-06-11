using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JobASP.Models.DTO;
using System.Text.Json;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using JobASP.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JobASP.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public JobController(IHttpClientFactory httpClientFactory)
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Job()
        {
            List<JobDTO> response = new List<JobDTO>();
            try
            {
                var client = _httpClientFactory.CreateClient();
                AddAuthorizationHeader(client);
                var httpResponse = await client.GetAsync("https://localhost:7264/api/Job/get-all-jobs");
                httpResponse.EnsureSuccessStatusCode();

                using (var responseStream = await httpResponse.Content.ReadAsStreamAsync())
                {
                    response = await JsonSerializer.DeserializeAsync<List<JobDTO>>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(AddJobRequestDTO addJobRequestDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                AddAuthorizationHeader(client);

                var json = JsonSerializer.Serialize(addJobRequestDTO);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var httpResponse = await client.PostAsync("https://localhost:7264/api/Job/add-job", content);
                httpResponse.EnsureSuccessStatusCode();

                return RedirectToAction("Job");
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Error = $"HTTP Request error: {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Job");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteJob(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid job ID.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                AddAuthorizationHeader(client);
                var httpResponse = await client.DeleteAsync($"https://localhost:7264/api/Job/delete-job-by-id/{id}");
                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Job");
                }
                else
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to delete job. Status code: {httpResponse.StatusCode}, Reason: {httpResponse.ReasonPhrase}, Response: {responseContent}");
                    ModelState.AddModelError(string.Empty, "Failed to delete job. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the job.");
            }

            return RedirectToAction("Job");
        }

        public async Task<IActionResult> Detail(int id)
        {
            JobDTO response = new JobDTO();
            try
            {
                var client = _httpClientFactory.CreateClient();
                AddAuthorizationHeader(client);
                var httpResponseMess = await client.GetAsync("https://localhost:7264/api/Job/get-job-by-id/" + id);
                httpResponseMess.EnsureSuccessStatusCode();
                var stringResponseBody = await httpResponseMess.Content.ReadAsStringAsync();
                response = await httpResponseMess.Content.ReadFromJsonAsync<JobDTO>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AddJobRequestDTO responseJob = null;
            var client = _httpClientFactory.CreateClient();
            AddAuthorizationHeader(client);
            try
            {
                var httpResponseMess = await client.GetAsync($"https://localhost:7264/api/Job/get-job-by-id/{id}");

                if (httpResponseMess.IsSuccessStatusCode)
                {
                    responseJob = await httpResponseMess.Content.ReadFromJsonAsync<AddJobRequestDTO>();
                }
                else
                {
                    ViewBag.Error = "Công việc không tồn tại hoặc có lỗi trong quá trình lấy dữ liệu.";
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

            return View(responseJob);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] AddJobRequestDTO job)
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
                        RequestUri = new Uri($"https://localhost:7264/api/Job/update-job-by-id/{id}"),
                        Content = new StringContent(JsonSerializer.Serialize(job), Encoding.UTF8, MediaTypeNames.Application.Json)
                    };
                    var httpResponseMess = await client.SendAsync(httpRequestMess);
                    httpResponseMess.EnsureSuccessStatusCode();

                    var updatedJob = await httpResponseMess.Content.ReadFromJsonAsync<AddJobRequestDTO>();
                    if (updatedJob != null)
                    {
                        return RedirectToAction("Job", "Job");
                    }
                    else
                    {
                        ViewBag.Error = "Không thể cập nhật công việc.";
                        return View("Error");
                    }
                }
                else
                {
                    return View(job);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchJobs(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var client = _httpClientFactory.CreateClient();
            AddAuthorizationHeader(client);
            var jobs = await client.GetFromJsonAsync<IEnumerable<JobDTO>>("https://localhost:7264/api/Job/get-all-jobs");

            if (!string.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(j =>
                    j.JobID.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    j.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    j.StartDate.ToString("dd-MM-yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    j.Status.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    j.StaffName.Any(s => s.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            return View("Job", jobs);
        }
    }
}
