using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace EmployeeManagement
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private Mock<HttpClient> _httpClientMock;
        private EmployeeService _employeeService;

        [SetUp]
        public void Setup()
        {
            _httpClientMock = new Mock<HttpClient>();
            _employeeService = new EmployeeService();
            _employeeService._httpClient = _httpClientMock.Object;
        }


        [Test]
        public async Task GetAllEmployees_Should_Return_Employees_List()
        {
            // Arrange
            var expectedResponse = new EmployeeApiResponse
            {
                Data = new List<Employee>
                {
                    new Employee { id = 1, name = "John", email = "Doe@gmail.com" ,status="active",gender="male"},
                    new Employee { id = 2, name = "Jane", email = "Doe" ,status="active",gender="male" }
                }
            };
            var json = JsonConvert.SerializeObject(expectedResponse);
            _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
      {
          Content = new StringContent(json)
      });

            // Act
            var result = await _employeeService.GetAllEmployees();

            // Assert
            Assert.AreEqual(expectedResponse.Data.Count, result.Count);
        }
        [Test]
        public async Task GetEmployeeById_ReturnsEmployee()
        {
            // Arrange
            var employee = new Employee { id = 1, name = "John", email = "Doe@gmail.com", status = "active", gender = "male" };
            var apiResponse = new EmployeeApiSearchResponse { Data = employee };
            _httpClientMock.Setup(c => c.GetAsync("users/1")).ReturnsAsync(new HttpResponseMessage { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(apiResponse)) });

            // Act
            var result = await _employeeService.GetEmployeeById(1);

            // Assert
            Assert.AreEqual(employee, result);
        }

        [Test]
        public async Task DeleteEmployee_ReturnsTrueWhenSuccessful()
        {
            // Arrange
            int id = 1;
            _httpClientMock.Setup(c => c.DeleteAsync($"users/{id}")).ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.NoContent });

            // Act
            var result = await _employeeService.DeleteEmployee(id);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateEmployee_ReturnsCreatedEmployee()
        {
            // Arrange
            var employee = new Employee { id = 1, name = "John", email = "Doe@gmail.com", status = "active", gender = "male" };
            var apiResponse = new EmployeeApiSearchResponse { Data = employee };
            _httpClientMock.Setup(c => c.PostAsync("users", It.IsAny<StringContent>())).ReturnsAsync(new HttpResponseMessage { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(apiResponse)) });

            // Act
            var result = await _employeeService.CreateEmployee(employee);

            // Assert
            Assert.AreEqual(employee, result);
        }

        [Test]
        public async Task GetTotalEmployees_ReturnsTotalNumberOfEmployees()
        {
            // Arrange
            var pagination = new ApiPagination { Total = 10 };
            var apiResponse = new EmployeeApiResponse { Meta = new ApiMeta { Pagination = pagination } };
            _httpClientMock.Setup(c => c.GetAsync("users")).ReturnsAsync(new HttpResponseMessage { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(apiResponse)) });

            // Act
            var result = await _employeeService.GetTotalEmployees();

            // Assert
            Assert.AreEqual(10, result);
        }
    }
}
