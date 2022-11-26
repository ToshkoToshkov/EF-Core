namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models;
    using ViewModels.Positions;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Services.DTO.Category;
    using FastFood.Services.DTO.position;
    using FastFood.Core.ViewModels.Employees;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            this.CreateMap<Position, EmployeeRegisterPositionAvailable>()
                .ForMember(x => x.PositionId, y => y.MapFrom(s => s.Id));

            //Categories

            this.CreateMap<CreateCategoryInputModel, CreateCategoryDTO>();

            this.CreateMap<ListAllCategoriesDTO, CategoryAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.CategoryName));
           
            this.CreateMap<CreateCategoryDTO, Category>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.CategoryName));

            this.CreateMap<Category, ListAllCategoriesDTO>()
                .ForMember(x => x.CategoryName, y => y.MapFrom(s => s.Name));

            //Employees

            this.CreateMap<EmployeeRegisterPositionAvailable, RegisterEmployeeViewModel>();
        }
    }
}
