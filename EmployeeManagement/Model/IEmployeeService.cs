using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Model
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployees();
        Task<List<Employee>> GetEmployeesByPage(int pageNumber);
        Task<Employee> GetEmployeeById(int id);
        Task<List<Employee>> GetEmployeesByName(string name);
        Task<bool> DeleteEmployee(int id);
        Task<Employee> CreateEmployee(Employee employee);
        Task<bool> UpdateEmployee(int Id, Employee employee);
        Task<int> GetTotalEmployees();
    }
}
