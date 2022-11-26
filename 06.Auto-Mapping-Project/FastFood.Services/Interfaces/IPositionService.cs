using System.Collections.Generic;
using FastFood.Services.DTO.position;

namespace FastFood.Services.Interfaces
{
    public interface IPositionService
    {
        ICollection<EmployeeRegisterPositionAvailable> GetPositionsAvailable();
    }
}
