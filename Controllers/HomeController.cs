using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ESP_Client.Models;
using System.Text.Json;


namespace ESP_Client.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private static HttpClient espClient = new()
    {
        BaseAddress = new Uri("https://developer.sepush.co.za/business/2.0/"),
    };
    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        if (!espClient.DefaultRequestHeaders.Contains("token"))
        {
            espClient.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("ESP_Token"));
        }
        HttpResponseMessage response = await espClient.GetAsync("api_allowance");
        var jsonResponse = await response.Content.ReadAsStringAsync();

        AllowanceResponse? deserialisedResponse = JsonSerializer.Deserialize<AllowanceResponse>(jsonResponse);
        return View(deserialisedResponse.allowance);


    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
