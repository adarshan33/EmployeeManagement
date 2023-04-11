using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EmployeeManagement.Model;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace EmployeeManagement.Tests
{
    [TestFixture]
    public class EmployeeServiceTests
    {

        private Mock<HttpMessageHandler> _mockHandler;
        private EmployeeService _employeeService;

        [SetUp]
        public void Setup()
        {
            _mockHandler = new Mock<HttpMessageHandler>();
            _employeeService = new EmployeeService(_mockHandler.Object);
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
            _mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent(json)
               });

            // Act
            var result = await _employeeService.GetAllEmployees();

            //
             Assert.That(result.Count, Is.EqualTo(expectedResponse.Data.Count));
        }

        [Test]
        public async Task GetEmployeeById_ReturnsEmployee()
        {
            // Arrange
            var employee = new Employee { id = 1, name = "John", email = "Doe@gmail.com", status = "active", gender = "male" };
            var apiResponse = new EmployeeApiSearchResponse { Data = employee };
            _mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(apiResponse)) });

            // Act
            var result = await _employeeService.GetEmployeeById(1);

            // Assert
            Assert.That(result.id, Is.EqualTo(employee.id));
        }

        [Test]
        public async Task DeleteEmployee_ReturnsFalseWhenNotSuccessful()
        {
            // Arrange
            int id = 1;
            var employee = new Employee { id = 1, name = "John", email = "Doe@gmail.com", status = "active", gender = "male" };
            var apiResponse = new EmployeeApiSearchResponse { Data = employee };
            _mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(apiResponse)) });

            // Act
            var result = await _employeeService.DeleteEmployee(id);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateEmployee_ReturnsCreatedEmployee()
        {
            // Arrange
            var employee = new Employee { id = 1, name = "John", email = "Doe@gmail.com", status = "active", gender = "male" };
            var apiResponse = new EmployeeApiSearchResponse { Data = employee };
            _mockHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(apiResponse)) });

            // Act
            var result = await _employeeService.CreateEmployee(employee);

            // Assert
            Assert.That(result.id, Is.EqualTo(employee.id));
            Assert.That(result.email, Is.EqualTo(employee.email));
            Assert.That(result.name, Is.EqualTo(employee.name));
        }

    }
}



