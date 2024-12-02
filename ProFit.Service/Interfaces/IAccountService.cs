using System.Security.Claims;
using ProFit.Domain.Models;
using ProFit.Domain.Response;

namespace ProFit.Service.Interfaces;

public interface IAccountService
{
    Task<BaseResponse<ClaimsIdentity>> Register(User model);
    Task<BaseResponse<ClaimsIdentity>> Login(User model);
}