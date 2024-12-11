using AutoMapper;
using ProFit.DAL.Interfaces;
using ProFit.Domain.Enum;
using ProFit.Domain.Filter;
using ProFit.Domain.Models;
using ProFit.Domain.ModelsDb;
using ProFit.Domain.Response;
using ProFit.Service.Interfaces;
using Exception = System.Exception;
using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Service;

public class ProductService:IProductService
{
    private readonly IBaseStorage<ProductDb> _productStorage;
    private readonly IBaseStorage<PictureProductDb> _pictureStorage;

    private IMapper _mapper { get; set; }

    private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
    {
        p.AddProfile<AppMappingProfile>();
    });

    public ProductService(IBaseStorage<ProductDb> productStorage)
    {
        _productStorage = productStorage;
        _mapper = mapperConfiguration.CreateMapper();
    }

    public BaseResponse<List<Product>> GetAllProducts(Type type)
    {
        try
        {
            var productDb = _productStorage.GetAll().Where(x=>type==x.Type).ToList();
            var result = _mapper.Map<List<Product>>(productDb);
            if (result.Count == 0)
            {
                return new BaseResponse<List<Product>>()
                {
                    Description = "Найдено 0 элементов",
                    StatusCode = StatusCode.Ok
                };
            }

            return new BaseResponse<List<Product>>()
            {
                Data = result,
                StatusCode = StatusCode.Ok
            };
        }
        catch(Exception ex)
        {
            return new BaseResponse<List<Product>>()
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }

    public BaseResponse<List<Product>> GetProductByFilter(ProductFilter filter)
    {
        try
        {
            var productsFilter = GetAllProducts(filter.Type).Data;
            if (filter != null && productsFilter != null)
            {
                if (filter.PriceAfultMax != 2000 || filter.PriceAfultMin != 0)
                {
                    productsFilter = productsFilter
                        .Where(f => f.Price < filter.PriceAfultMax && f.Price > filter.PriceAfultMin).ToList();
                }
            }

            return new BaseResponse<List<Product>>
            {
                Data = productsFilter,
                Description = "Отфильтрованные данные",
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<Product>>
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }

    public async Task<BaseResponse<Product>> GetProductById(Guid id)
    {
        try
        {
            var productDb = await _productStorage.Get(id);

            var result = _mapper.Map<Product>(productDb);
            if (result == null)
            {
                return new BaseResponse<Product>()
                {
                    Description = "Найдено 0 элементов",
                    StatusCode = StatusCode.Ok
                };
            }

            return new BaseResponse<Product>()
            {
                Data = result,
                StatusCode = StatusCode.Ok
            };
        }
        catch(Exception ex)
        {
            return new BaseResponse<Product>
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }

    public BaseResponse<List<PictureProduct>> GetPictureByIdProduct(Guid id)
    {
        try
        {
            var pictureDb = _pictureStorage.GetAll().Where(x => id == x.IdProduct).ToList();

            var result = _mapper.Map<List<PictureProduct>>(pictureDb);
            if (result.Count==0)
            {
                return new BaseResponse<List<PictureProduct>>
                {
                    Description = "Найдено 0 элементов",
                    StatusCode = StatusCode.Ok
                };
            }
            return new BaseResponse<List<PictureProduct>>
            {
                Data = result,
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<PictureProduct>>
            {
                Description = ex.Message,
                StatusCode = StatusCode.IternalServerError
            };
        }
    }
}