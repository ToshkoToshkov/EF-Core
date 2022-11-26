namespace FastFood.Core.Controllers
{

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Categories;
    using System.Linq;
    using System.Collections.Generic;
    using Services.Interfaces;
    using Services.DTO.Category;

    public class CategoriesController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;

        public CategoriesController(IMapper mapper ,ICategoryService categoryService)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Create");
            }

            CreateCategoryDTO categoryDto = 
                this.mapper.Map<CreateCategoryDTO>(model);

            this.categoryService.Create(categoryDto);

            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            ICollection<ListAllCategoriesDTO> categoriesDto =
                this.categoryService.All();

            List<CategoryAllViewModel> categoryViewModels =
                this.mapper.Map<ICollection<ListAllCategoriesDTO>,
                ICollection<CategoryAllViewModel>>(categoriesDto)
                .ToList();

            return this.View("All", categoryViewModels);
        }
    }
}
