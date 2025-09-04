using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.DTOs;
using EMS.Manager.Interfaces;
using Shouldly;
using Xunit;

namespace EMS.Tests.Employees
{
    public class EmployeeAppService_Tests : EMSTestBase
    {
        private readonly IEmployeeAppService _employeeAppService;

        public EmployeeAppService_Tests()
        {
            _employeeAppService = Resolve<IEmployeeAppService>();
        }

     [Fact]
public async Task Should_Create_Employee()
{
    // Arrange
    var employee = new CreateEmployeeDto
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com"
    };

    // Act
    var id = await _employeeAppService.CreateAsync(employee);

    // Assert
    id.ShouldNotBe(Guid.Empty);

    // Verify that the employee was actually saved in the database
    var createdEmployee = await _employeeAppService.GetByIdAsync(id);
    createdEmployee.ShouldNotBeNull();
    createdEmployee.FirstName.ShouldBe("John");
    createdEmployee.LastName.ShouldBe("Doe");
    createdEmployee.Email.ShouldBe("john.doe@example.com");
}





        [Fact]
        public async Task Should_Get_Employee_ById()
        {
            // Arrange
            var employee = new CreateEmployeeDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com"
            };

            var id = await _employeeAppService.CreateAsync(employee);

            // Act
            var result = await _employeeAppService.GetByIdAsync(id);

            // Assert
            result.ShouldNotBeNull();
            // result.Id.ShouldBe(id); // CreateEmployeeDto does not have an Id property
            result.FirstName.ShouldBe("Jane");
            result.LastName.ShouldBe("Smith");
            result.Email.ShouldBe("jane.smith@example.com");
        }

        [Fact]
        public async Task Should_Get_All_Employees()
        {
            // Arrange
            var emp1 = new CreateEmployeeDto { FirstName = "Akhil", LastName = "Pandey", Email = "akhil.pandey@example.com" };
            var emp2 = new CreateEmployeeDto { FirstName = "Maya", LastName = "Sharma", Email = "maya.sharma@example.com" };

            await _employeeAppService.CreateAsync(emp1);
            await _employeeAppService.CreateAsync(emp2);

            // Act
            var allEmployees = await _employeeAppService.GetAllAsync();

            // Assert
            allEmployees.Count.ShouldBeGreaterThanOrEqualTo(2);
            allEmployees.Any(e => e.FirstName == "Akhil" && e.LastName == "Pandey").ShouldBeTrue();
            allEmployees.Any(e => e.FirstName == "Maya" && e.LastName == "Sharma").ShouldBeTrue();
        }
    }
}
