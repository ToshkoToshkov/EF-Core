using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Services.Interfaces
{
    using DTO.Category;
    public interface ICategoryService
    {
        void Create(CreateCategoryDTO dto);

        ICollection<ListAllCategoriesDTO> All();
    }
}
