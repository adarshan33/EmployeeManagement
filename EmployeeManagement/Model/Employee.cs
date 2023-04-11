using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Model
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string status { get; set; }
    }
    public class EmployeeApiResponse
    {
        public int Code { get; set; }
        public ApiMeta? Meta { get; set; }
        public List<Employee> Data { get; set; }

    }

    public class ApiMeta
    {
        public ApiPagination Pagination { get; set; }
    }

    public class ApiPagination
    {
        public int Total { get; set; }
        public int Pages { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }

    public class EmployeeApiSearchResponse
    {
        public int Code { get; set; }
        public object? Meta { get; set; }
        public Employee Data { get; set; }
    }

}
