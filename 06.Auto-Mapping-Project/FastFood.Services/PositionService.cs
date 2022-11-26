using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Services.DTO.position;
using FastFood.Services.Interfaces;
using FastFood.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace FastFood.Services
{
    public class PositionService : IPositionService
    {
        private readonly FastFoodContext dbContext;
        private readonly IMapper mapper;

        public PositionService(FastFoodContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public ICollection<EmployeeRegisterPositionAvailable> GetPositionsAvailable()
            => this.dbContext
                   .Positions
                   .ProjectTo<EmployeeRegisterPositionAvailable>
                             (this.mapper.ConfigurationProvider)
                   .ToList();
    }
}
