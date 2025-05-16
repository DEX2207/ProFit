using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using ProFit.DAL.Interfaces;
using ProFit.Domain.Enum;
using ProFit.Domain.Helpers;
using ProFit.Domain.Models;
using ProFit.Domain.ModelsDb;
using ProFit.Domain.Response;
using ProFit.Service.Interfaces;
using ProFit.Domain.Validators;
using MailKit.Net.Smtp;


namespace ProFit.Service;

public class AccountService:IAccountService
{
    private readonly IBaseStorage<UserDb> _userStorage;

    private IMapper _mapper { get; set; }

    private UserValidator _validationRules { get; set; }

    private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
    {
        p.AddProfile<AppMappingProfile>();
    });

    public AccountService(IBaseStorage<UserDb> userStorage)
    {
        _userStorage = userStorage;
        _mapper = mapperConfiguration.CreateMapper();
        _validationRules = new UserValidator();
    }

    public async Task<BaseResponse<ClaimsIdentity>> Login(User model)
    {
        try
        {
            await _validationRules.ValidateAndThrowAsync(model);

            var userDb = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);
            if (userDb == null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Пользователь не найден"

                };
            }

            if (userDb.Password != HashPasswordHelper.HashPassword(model.Password))
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Неверный пароль"
                };
            }

            model = _mapper.Map<User>(userDb);
            var result = AuthenticateUserHelper.Authenticate(model);

            return new BaseResponse<ClaimsIdentity>()
            {
                Data = result,
                StatusCode = StatusCode.Ok
            };
        }
        catch (ValidationException ex)
        {
            var errorMessages = string.Join(";", ex.Errors.Select(e => e.ErrorMessage));

            return new BaseResponse<ClaimsIdentity>()
            {
                Description = errorMessages,
                StatusCode = StatusCode.BadRequest
            };
        }
        catch(Exception ex)
        {
            return new BaseResponse<ClaimsIdentity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }
    public async Task<BaseResponse<string>> Register(User model)
    {
        try
        {
            Random random = new Random();
            string confirmationCode = $"{random.Next(10)}{random.Next(10)}{random.Next(10)}{random.Next(10)}";

            if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) != null)
            {
                return new BaseResponse<string>()
                {
                    Description = "Пользователь с такой почтой уже есть"
                };
            }

            await SendEmail(model.Email, confirmationCode);

            var result = AuthenticateUserHelper.Authenticate(model);

            return new BaseResponse<string>()
            {
                Data = confirmationCode,
                Description = "Письмо отправлено",
                StatusCode = StatusCode.Ok
            };
        }
        catch (ValidationException ex)
        {
            var errorMessages = string.Join(";", ex.Errors.Select(e => e.ErrorMessage));
            return new BaseResponse<string>()
            {
                Description = errorMessages,
                StatusCode = StatusCode.BadRequest
            };
        }
        catch(Exception ex)
        {
            return new BaseResponse<string>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }

    public async Task SendEmail(string email, string confirmationCode)
    {
        string path = "D:\\INTER (E)\\1111\\учебная практика\\passwordPractice.txt";

        var emailMessage = new MimeMessage();
        
        emailMessage.From.Add(new MailboxAddress("Администрация сайта","ProFit"));
        emailMessage.To.Add(new MailboxAddress("",email));
        emailMessage.Subject = "Подтверждение адреса электронной почты";
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text="<html>"+"<head>"+"<style>"+
                 "body{font-family:Arial,sans-serif; background-color:#f2f2f2;}"+
                 ".container{max-width: 600px;margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1)}"+
                 ".header {text-alight: center; margin-bottom: 20px;}"+
                 ".message {font-size: 16px; line-height: 1.6;}"+
                 ".container-code {background-color: #f0f0f0; padding: 5px; border-radius: 5px; font-weight: bold; }"+
                 ".code {text-alight: center;}"+
                 "</style>"+
                 "</head>"+
                 "<body>"+
                 "<div class='container'>"+
                 "<div class='header'><h1>Для прохождения регистрации введите код регистрации, не передавайте его никому. Если вы не регистрировались, проигнорируйте данное письмо.</h1></div>"+
                 "<div class='message'>"+
                 "<p>Ваш код:</p>"+
                 "<div class='container-code'><p class='code'>" + confirmationCode + "</p></div>"+
                 "</div>"+"</div>"+"</body>"+"</html>"
        };
        using (StreamReader reader = new StreamReader(path))
        {
            string password = await reader.ReadToEndAsync();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("profitcenter803@gmail.com", password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }

    public async Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode)
    {
        try
        {
            if (code != confirmCode)
            {
                throw new Exception("Неверный код! Регистрация не выполнена");
            }
            
            model.CreatedAt=DateTime.Now;
            model.Password = HashPasswordHelper.HashPassword(model.Password);

            await _validationRules.ValidateAndThrowAsync(model);

            var userDb = _mapper.Map<UserDb>(model);

            await _userStorage.Add(userDb);

            var result = AuthenticateUserHelper.Authenticate(model);

            return new BaseResponse<ClaimsIdentity>()
            {
                Data = result,
                Description = "Объект добавился",
                StatusCode = StatusCode.Ok
            };
        }
        catch(Exception ex)
        {
            return new BaseResponse<ClaimsIdentity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }

    public async Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model)
    {
        try
        {
            var userDb = new UserDb();
            if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) == null)
            {
                model.Password = "google";
                model.CreatedAt = DateTime.Now;

                userDb = _mapper.Map<UserDb>(model);

                await _userStorage.Add(userDb);

                var resultRegister = AuthenticateUserHelper.Authenticate(model);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = resultRegister,
                    Description = "Объект добавился",
                    StatusCode = StatusCode.Ok
                };
            }

            var resultLogin = AuthenticateUserHelper.Authenticate(model);
            return new BaseResponse<ClaimsIdentity>()
            {
                Data = resultLogin,
                Description = "Объект уже был создан",
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<ClaimsIdentity>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }

    public User GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        var user = _userStorage.GetAll().FirstOrDefault(u=>u.Email== email);
        User users = _mapper.Map<User>(user);
        return users;
    }
}