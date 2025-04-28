using ESP_Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net;

namespace ESP_Client.Controllers
{
    public class ESPController : Controller
    {
        private static HttpClient espClient = new()
        {
            BaseAddress = new Uri("https://developer.sepush.co.za/business/2.0/"),
        };

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string search)
        {
            if (!espClient.DefaultRequestHeaders.Contains("token"))
            {
                espClient.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("ESP_Token"));
            }
            HttpResponseMessage response = await espClient.GetAsync("areas_search?text=" + search);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) // 200 OK
            {
                AreasModel? deserializedResponse = JsonSerializer.Deserialize<AreasModel>(jsonResponse);
                return View(deserializedResponse);
            }

            if (response.StatusCode == HttpStatusCode.NotFound) // 404
            {
                ModelState.AddModelError(string.Empty, "Area not found.");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden) // 403
            {
                ModelState.AddModelError(string.Empty, "Access denied.");
            }
            else if (response.StatusCode == HttpStatusCode.TooManyRequests) // 429
            {
                ModelState.AddModelError(string.Empty, "Too many requests. Please try again later.");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError) // 500
            {
                ModelState.AddModelError(string.Empty, "Server error, please try again later.");
            }

            return View();
        }

        public async Task<IActionResult> Details(string id)
        {
            if (!espClient.DefaultRequestHeaders.Contains("token"))
            {
                espClient.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("ESP_Token"));
            }

            HttpResponseMessage response = await espClient.GetAsync("area?id=" + id);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) // 200 OK
            {
                AreasModel? deserializedResponse = JsonSerializer.Deserialize<AreasModel>(jsonResponse);
                return View(deserializedResponse);
            }

            if (response.StatusCode == HttpStatusCode.NotFound) // 404
            {
                ModelState.AddModelError(string.Empty, "Area not found.");
            }
            else if (response.StatusCode == HttpStatusCode.Forbidden) // 403
            {
                ModelState.AddModelError(string.Empty, "Access denied.");
            }
            else if (response.StatusCode == HttpStatusCode.TooManyRequests) // 429
            {
                ModelState.AddModelError(string.Empty, "Too many requests. Please try again later.");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError) // 500
            {
                ModelState.AddModelError(string.Empty, "Server error, please try again later.");
            }

            return View();
        }

    }
}