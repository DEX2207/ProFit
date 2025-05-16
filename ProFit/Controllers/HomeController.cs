using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProFit.Domain.ViewModels.LoginAndRegistration;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using ProFit.Domain.Models;
using ProFit.Domain.ViewModels;
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

    private readonly IWebHostEnvironment _appEnvironment;

    public HomeController(ILogger<HomeController> logger, IAccountService accountService,IWebHostEnvironment appEnvironment)
    {
        _logger = logger;
        _mapper = mapperConfiguration.CreateMapper();
        _accountService = accountService;
        _appEnvironment = appEnvironment;
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
            if (code.StatusCode == Domain.Enum.StatusCode.Ok)
            {
                return Ok(confirm);
            }
            ModelState.AddModelError("",code.Description);
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

    public async Task AuthenticationGoogle(string returnUrl = "/")
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("GoogleResponse",new {returnUrl}),
            Parameters = { {"prompt","select_account"} }
        });
    }

    public async Task<IActionResult> GoogleResponse(string returnUrl="/")
    {
        try
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Succeeded == true)
            {
                User model = new User
                {
                    Login = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                    Email = result.Principal.FindFirst(ClaimTypes.Email)?.Value
                };
                var response = await _accountService.IsCreatedAccount(model);

                if (response.StatusCode == Domain.Enum.StatusCode.Ok)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));
                    return Redirect(returnUrl);
                }
            }

            return BadRequest("Аутентификация не удалась");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    public IActionResult Profile()
    {
        if (User.Identity.IsAuthenticated)
        {
            var email = User.Identity.Name;
            var user = _accountService.GetUserByEmail(email);
            if (user != null)
            {
                var model = new ProfileViewModel
                {
                    Login = user.Login,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt
                };
                return View(model);
            }
        }
        return RedirectToAction("Login", "Account");
    }
}