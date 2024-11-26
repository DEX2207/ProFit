using Microsoft.AspNetCore.Mvc;
using ProFit.Domain.ViewModels.LoginAndRegistration;

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

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Services()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Login([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            return Ok(model);
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
        return BadRequest(errors);
    }

    [HttpPost]
    public IActionResult Register([FromBody] RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(errors);
        }

        return Ok(model);
    }
}