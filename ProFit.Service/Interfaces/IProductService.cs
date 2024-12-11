using ProFit.Domain.Filter;
using ProFit.Domain.Models;
using ProFit.Domain.Response;
using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Service.Interfaces;

public interface IProductService
{
    BaseResponse<List<Product>> GetAllProducts(Type type);
    BaseResponse<List<Product>> GetProductByFilter(ProductFilter filter);

    Task<BaseResponse<Product>> GetProductById(Guid id);

    BaseResponse<List<PictureProduct>> GetPictureByIdProduct(Guid id);

}