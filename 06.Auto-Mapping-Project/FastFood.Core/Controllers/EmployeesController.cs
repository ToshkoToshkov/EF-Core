namespace FastFood.Core.Controllers
{
    using System;
    using AutoMapper;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Employees;
    using FastFood.Services.Interfaces;
    using System.Collections.Generic;
    using FastFood.Services.DTO.position;

    public class EmployeesController : Controller
    {
        private readonly IPositionService positionService;
        private readonly IMapper mapper;

        public EmployeesController(IMapper mapper ,IPositionService positionService)
        {
            this.mapper = mapper;
            this.positionService = positionService;
        }

        public IActionResult Register()
        {
            ICollection<EmployeeRegisterPositionAvailable> positionDto =
                this.positionService.GetPositionsAvailable();

            List<RegisterEmployeeViewModel> regViewModel =
                    this.mapper
                    .Map<ICollection<EmployeeRegisterPositionAvailable>,
                    ICollection<RegisterEmployeeViewModel>>(positionDto)
                    .ToList();

            return this.View(regViewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterEmployeeInputModel model)
        {
            throw new NotImplementedException();
        }

        public IActionResult All()
        {
            throw new NotImplementedException();
        }
    }
}
