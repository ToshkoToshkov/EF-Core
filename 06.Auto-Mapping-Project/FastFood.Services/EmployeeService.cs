using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Services.DTO.Employee;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class EmployeeService : IEmployeeService
    {
        public ICollection<ListAllEmployeesDto> All()
        {
            throw new NotImplementedException();
        }

        public void Register(RegisterEmployeeDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
