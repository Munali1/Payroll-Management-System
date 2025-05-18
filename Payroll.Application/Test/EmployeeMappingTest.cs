using Payroll.Domain.Entities;

using AutoMapper;
using Payroll.Application.Mapper;
using Xunit;
using Payroll.Application.ViewModels;

namespace Payroll.Tests
{
    public class EmployeeMappingTests
    {
        private readonly IMapper mapper;
        public EmployeeMappingTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
          
            });
            mapper = config.CreateMapper();
        }
        [Fact]
        public void Should_Map_Employee_To_EmployeeViewModel()
        {
            // Arrange
            var employee = new Employee
            {
                UserId = "user001",
                DepartmentId = 5,
                Designation = "Backend Developer",
                EmployeeImage = "img123.jpg"
            };

            // Act
            var result = mapper.Map<EmployeeViewModel>(employee);

            // Assert
            Assert.Equal(employee.UserId, result.UserId);
            Assert.Equal(employee.DepartmentId, result.DepartmentId);
            Assert.Equal(employee.Designation, result.Designation);
            Assert.Equal(employee.EmployeeImage, result.EmployeeImage);
        }

        [Fact]
        public void Should_Map_EmployeeViewModel_To_Employee()
        {
            // Arrange
            var viewModel = new EmployeeViewModel
            {
                UserId = "user002",
                DepartmentId = 3,
                Designation = "QA Engineer",
                EmployeeImage = "qa.jpg"
            };

            // Act
            var result = mapper.Map<Employee>(viewModel);

            // Assert
            Assert.Equal(viewModel.UserId, result.UserId);
            Assert.Equal(viewModel.DepartmentId, result.DepartmentId);
            Assert.Equal(viewModel.Designation, result.Designation);
            Assert.Equal(viewModel.EmployeeImage, result.EmployeeImage);
        }
    }
}
