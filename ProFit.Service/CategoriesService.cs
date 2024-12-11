using AutoMapper;
using ProFit.DAL.Interfaces;
using ProFit.Domain.Enum;
using ProFit.Domain.Models;
using ProFit.Domain.ModelsDb;
using ProFit.Domain.Response;
using ProFit.Service.Interfaces;

namespace ProFit.Service;

public class CategoriesService:ICategoriesService
{
    private readonly IBaseStorage<CategoriesDb> _categoriesStorage;

    private IMapper _mapper { get; set; }
    

    private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
    {
        p.AddProfile<AppMappingProfile>();
    });

    public CategoriesService(IBaseStorage<CategoriesDb> categoriesStorage)
    {
        _categoriesStorage = categoriesStorage;
        _mapper = mapperConfiguration.CreateMapper();
    }

    public BaseResponse<List<Categories>> GetAllCategories()
    {
        try
        {
            var categoriesDb = _categoriesStorage.GetAll().ToList();

            var result = _mapper.Map<List<Categories>>(categoriesDb);
            if (result.Count == 0)
            {
                return new BaseResponse<List<Categories>>()
                {
                    Description = "Найдено 0 элементов",
                    StatusCode = StatusCode.Ok
                };
            }

            return new BaseResponse<List<Categories>>()
            {
                Data = result,
                StatusCode = StatusCode.Ok
            };
        }
        catch(Exception ex)
        {
            return new BaseResponse<List<Categories>>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }
}