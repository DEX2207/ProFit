using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProFit.Domain.ViewModels;
using ProFit.Service;
using ProFit.Service.Interfaces;

namespace ProFit.Controllers;

public class CategoriesController:Controller
{
   private readonly ICategoriesService _categoriesService;

   private IMapper _mapper { get; set; }

   private MapperConfiguration mapperConfiguration = new MapperConfiguration(p =>
   {
      p.AddProfile<AppMappingProfile>();
   });

   public CategoriesController(ICategoriesService categoriesService)
   {
      _categoriesService = categoriesService;
      _mapper = mapperConfiguration.CreateMapper();
   }
   public IActionResult Categories()
   {
      var result = _categoriesService.GetAllCategories();
      var categories = _mapper.Map<List<CategoriesViewModel>>(result.Data);
      return View(categories);
   }
}