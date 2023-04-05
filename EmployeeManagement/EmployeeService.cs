using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class EmployeeService
    {
        internal HttpClient _httpClient;

        public EmployeeService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://gorest.co.in/public-api/");
            string apiToken = ConfigurationManager.AppSettings["ApiToken"];
            _httpClient.DefaultRequestHeaders.Authorization =
             new AuthenticationHeaderValue("Bearer", apiToken);
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var response = await _httpClient.GetAsync("users");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            EmployeeApiResponse apiResponse = JsonConvert.DeserializeObject<EmployeeApiResponse>(json);
            return apiResponse.Data;
        }

        public async Task<List<Employee>> GetEmployeesByPage(int pageNumber)
        {
            var response = await _httpClient.GetAsync($"users?page={pageNumber}");
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

        public async Task<bool> DeleteEmployee(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"users/{id}");
                var json = await response.Content.ReadAsStringAsync();
                EmployeeApiSearchResponse employee = JsonConvert.DeserializeObject<EmployeeApiSearchResponse>(json);
                if (employee.Data == null)
                {
                    return true;
                }
                return false;
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

        public async Task<bool> UpdateEmployee(int Id,Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"users/{Id}", content);
            var updatedResponse = await response.Content.ReadAsStringAsync();
            EmployeeApiSearchResponse result = JsonConvert.DeserializeObject<EmployeeApiSearchResponse>(updatedResponse);
            if(result.Code == 200) 
            {
                return true;
            }        
            return false;
        }


        public async Task<int> GetTotalEmployees()
        {
            var response = await _httpClient.GetAsync("users");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            EmployeeApiResponse apiResponse = JsonConvert.DeserializeObject<EmployeeApiResponse>(json);
            return apiResponse.Meta.Pagination.Total;
        }
    }
}
