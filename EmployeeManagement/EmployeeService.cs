using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://gorest.co.in/public-api/");
            _httpClient.DefaultRequestHeaders.Authorization =
             new AuthenticationHeaderValue("Bearer", "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56");
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var response = await _httpClient.GetAsync("users");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            EmployeeApiResponse apiResponse = JsonConvert.DeserializeObject<EmployeeApiResponse>(json);
            return apiResponse.Data;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var response = await _httpClient.GetAsync($"users/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            EmployeeApiSearchResponse employee = JsonConvert.DeserializeObject<EmployeeApiSearchResponse>(json);
            return employee.Data;
        }

        public async Task<List<Employee>> GetEmployeesByName(string name)
        {
            var response = await _httpClient.GetAsync($"users?first_name={name}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            EmployeeApiResponse apiResponse = JsonConvert.DeserializeObject<EmployeeApiResponse>(json);
            return apiResponse.Data;
        }

        public async Task<bool> DeleteEmployee(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"users/{id}");
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                // Handle exception
                return false;
            }
        }

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("users", content);
            response.EnsureSuccessStatusCode();
            json = await response.Content.ReadAsStringAsync();
            var emp = JsonConvert.DeserializeObject<EmployeeApiSearchResponse>(json);
            return emp.Data;
        }
    }
}
