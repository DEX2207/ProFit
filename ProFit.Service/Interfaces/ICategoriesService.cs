using ProFit.Domain.Models;
using ProFit.Domain.Response;

namespace ProFit.Service.Interfaces;

public interface ICategoriesService
{
    BaseResponse<List<Categories>> GetAllCategories();
}