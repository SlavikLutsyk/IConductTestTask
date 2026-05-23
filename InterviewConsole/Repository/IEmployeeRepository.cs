using EmployeeService.Implementation.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeService.Implementation.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllActiveEmployeesAsync(CancellationToken cancellationToken);

        Task<EmployeeDto> UpdateEnableEmployeeAsync(int id, int enable, CancellationToken cancellationToken);
    }
}
