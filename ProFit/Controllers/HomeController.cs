using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProFit.Domain.ViewModels.LoginAndRegistration;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProFit.Domain.Models;
using ProFit.Service;
using ProFit.Service.Interfaces;

namespace ProFit.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IMapper _mapper { get; set; }
    private IAccountService _accountService { get; set; }
    private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
    {
        p.AddProfile<AppMappingProfile>();
    });

    public HomeController(ILogger<HomeController> logger, IAccountService acountService)
    {
        _logger = logger;
        _mapper = mapperConfiguration.CreateMapper();
        _accountService = acountService;
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
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(model);

            var response = await _accountService.Login(user);
            if (response.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));
                
                return Ok(model);
            }
            
            ModelState.AddModelError("",response.Description);
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
        return BadRequest(errors);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(model);

            var confirm = _mapper.Map<ConfirmEmailViewModel>(model);

            var code = await _accountService.Register(user);

            confirm.GeneratedCode = code.Data;
            
            return Ok(confirm);
        }
        
        var errors = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
        return BadRequest(errors);
    }

    [HttpPost]

    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel confirmEmailViewModel)
    {
        var user = _mapper.Map<User>(confirmEmailViewModel);

        var response = await _accountService.ConfirmEmail(user,confirmEmailViewModel.GeneratedCode,confirmEmailViewModel.CodeConfirm);

        if (response.StatusCode==Domain.Enum.StatusCode.Ok)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(response.Data));
            return Ok(confirmEmailViewModel);
        }
        ModelState.AddModelError("",response.Description);

        var errors = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
        return BadRequest(errors);
    }
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("SiteInformation", "Home");
    }
}