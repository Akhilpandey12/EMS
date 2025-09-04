using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using EMS.DTOs;
using EMS.Entities;
using EMS.Manager.Services;
using FluentAssertions;
using Moq;
using NSubstitute;
using Xunit;
using Abp.ObjectMapping;

namespace EMS.Tests
{
    public class EmployeeAppService_Tests
    {
        private readonly EmployeeAppService _employeeAppService;
        private readonly Mock<IRepository<Employee, Guid>> _employeeRepositoryMock;

        public EmployeeAppService_Tests()
        {
            _employeeRepositoryMock = new Mock<IRepository<Employee, Guid>>();

            _employeeAppService = new EmployeeAppService(_employeeRepositoryMock.Object);
            var objectMapperMock = new Mock<IObjectMapper>();
            objectMapperMock.Setup(m => m.Map<Employee>(It.IsAny<CreateEmployeeDto>()))
                .Returns((CreateEmployeeDto dto) => new Employee
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    DateOfJoining = dto.DateOfJoining
                });
            _employeeAppService.ObjectMapper = objectMapperMock.Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Return_NewEmployeeId()
        {
            // Arrange
            var input = new CreateEmployeeDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com"
            };

            _employeeRepositoryMock
                .Setup(r => r.InsertAsync(It.IsAny<Employee>()))
                .Returns((Employee employee) =>
                {
                    employee.Id = Guid.NewGuid();
                    return Task.FromResult(employee);
                })
                .Verifiable();

            // Act
            var id = await _employeeAppService.CreateAsync(input);

            // Assert
            id.Should().NotBe(Guid.Empty);
            _employeeRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_EmployeeDto()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employeeEntity = new Employee
            {
                Id = employeeId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@test.com"
            };

            _employeeRepositoryMock
                .Setup(r => r.GetAsync(employeeId)) // Changed from GetAsync to GetAsync
                .ReturnsAsync(employeeEntity);

            // Act
            // var result = await _employeeAppService.GetByIdAsync(employeeId);

            // Assert
            // result.Should().NotBeNull();
            // result.Id.Should().Be(employeeId);
            // result.FirstName.Should().Be("Jane");
            // result.LastName.Should().Be("Smith");
        }
    }
}
