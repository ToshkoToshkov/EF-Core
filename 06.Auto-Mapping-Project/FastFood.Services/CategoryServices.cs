using System;

namespace FastFood.Services
{
    using System.Collections.Generic;
    using AutoMapper;
    using Data;
    using DTO.Category;
    using Interfaces;
    using Models;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    
    public class CategoryServices : ICategoryService
    {
        private readonly FastFoodContext dbContext;
        private readonly IMapper mapper;

        public CategoryServices(FastFoodContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public ICollection<ListAllCategoriesDTO> All()
            => this.dbContext
                   .Categories
                   .ProjectTo<ListAllCategoriesDTO>
                             (this.mapper.ConfigurationProvider)
                   .ToList();

        public void Create(CreateCategoryDTO dto)
        {
            Category category = this.mapper.Map<Category>(dto);

            this.dbContext.Categories.Add(category);
            this.dbContext.SaveChanges();
        }
    }
}
