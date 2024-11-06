using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProFit.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult SiteInformation()
    {
        return View();
    }
}