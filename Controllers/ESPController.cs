using ESP_Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

            AreasModel? deserialisedResponse = JsonSerializer.Deserialize<AreasModel>(jsonResponse);
            return View(deserialisedResponse);

            // To Do: Cater for different response codes: 200, 404, 403, 429, 500
        }

        public async Task<IActionResult> Details(string id)
        {
            if (!espClient.DefaultRequestHeaders.Contains("token"))
            {
                espClient.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("ESP_Token"));
            }

            HttpResponseMessage response = await espClient.GetAsync("area?id=" + id);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            EspResponse? deserialisedResponse = JsonSerializer.Deserialize<EspResponse>(jsonResponse);
            return View(deserialisedResponse);

            // To Do: Cater for different response codes: 200, 404, 403, 429, 500
        }

    }
}