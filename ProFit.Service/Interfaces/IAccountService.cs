using System.Security.Claims;
using ProFit.Domain.Models;
using ProFit.Domain.Response;

namespace ProFit.Service.Interfaces;

public interface IAccountService
{
    Task<BaseResponse<string>> Register(User model);
    Task<BaseResponse<ClaimsIdentity>> Login(User model);

    Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode);

    Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model);
}