using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Services.Interfaces
{
    using Services.DTO.Employee;

    public interface IEmployeeService
    {
        void Register(RegisterEmployeeDto dto);

        ICollection<ListAllEmployeesDto> All();
    }
}
