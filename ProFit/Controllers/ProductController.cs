using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProFit.Domain.Filter;
using ProFit.Domain.ViewModels;
using ProFit.Service;
using ProFit.Service.Interfaces;
using Type = ProFit.Domain.Enum.Type;

namespace ProFit.Controllers;

public class ProductController:Controller
{
    private readonly IProductService _productService;
    private IMapper _mapper { get; set; }
    
    MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
    {
        p.AddProfile<AppMappingProfile>();
    });

    public ProductController(IProductService productService)
    {
        _productService = productService;
        _mapper = mapperConfiguration.CreateMapper();
    }

    public IActionResult Services(Type type)
    {
        var result = _productService.GetAllProducts(type);
        ListOfProductsViewModel listProduct = new ListOfProductsViewModel
        {
            Products = _mapper.Map<List<ProductForListOfProductsViewModel>>(result.Data),
            Type = type
        };
        return View(listProduct);
    }

    [HttpPost]
    public async Task<IActionResult> Filter([FromBody] ProductFilter filter)
    {
        var result = _productService.GetProductByFilter(filter);
        var filterProducts = _mapper.Map<List<ProductForListOfProductsViewModel>>(result.Data);
        return Json(filterProducts);
    }

    public async Task<IActionResult> ProductPage(Guid id)
    {
        var resultProduct = await _productService.GetProductById(id);
        var resultPicture = _productService.GetPictureByIdProduct(id);

        ProductPageViewModel product = _mapper.Map<ProductPageViewModel>(resultProduct.Data);
        product.PictureProducts = _mapper.Map<List<PictureProductViewModel>>(resultPicture.Data);

        return View(product);
    }
}