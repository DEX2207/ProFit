using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProFit.DAL.Interfaces;
using ProFit.Domain.Enum;
using ProFit.Domain.Helpers;
using ProFit.Domain.Models;
using ProFit.Domain.ModelsDb;
using ProFit.Domain.Response;
using ProFit.Service.Interfaces;
using ProFit.Domain.Validators;

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
    public async Task<BaseResponse<ClaimsIdentity>> Register(User model)
    {
        try
        {
            model.CreatedAt = DateTime.Now;
            model.Password = HashPasswordHelper.HashPassword(model.Password);

            await _validationRules.ValidateAndThrowAsync(model);

            var userDb = _mapper.Map<UserDb>(model);

            if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) != null)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = "Пользователь с такой почтой уже есть"
                };
            }

            await _userStorage.Add(userDb);

            var result = AuthenticateUserHelper.Authenticate(model);

            return new BaseResponse<ClaimsIdentity>()
            {
                Data = result,
                Description = "Пользователь зарегистрирован",
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
}