

using AutoMapper;
using Payroll.Application.Mapper;
using Payroll.Application.ViewModels;
using Payroll.Domain.Entities;
using Xunit;

namespace Payroll.Application.Test
{
    public class DepartmentMappingTest
    {
        private readonly IMapper mapper;
        public DepartmentMappingTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();

            });
            mapper = config.CreateMapper();
        }
        [Fact]
        public void Should_Map_Department_To_DepartmentViewModel()
        {
            // Arrange
            var department = new Department
            {
                DepartmentId = 1,
                DepartmentName = "Human Resources",
                DepartmentDescription = "Handles HR related activities",
                DepartmentEmail = "hr@company.com"
            };

            // Act
            var viewModel = mapper.Map<DepartmentViewModel>(department);

            // Assert
            Assert.Equal(department.DepartmentName, viewModel.DepartmentName);
            Assert.Equal(department.DepartmentDescription, viewModel.DepartmentDescription);
            Assert.Equal(department.DepartmentEmail, viewModel.DepartmentEmail);
        }

        [Fact]
        public void Should_Map_DepartmentViewModel_To_Department()
        {
            // Arrange
            var viewModel = new DepartmentViewModel
            {
                DepartmentName = "Finance",
                DepartmentDescription = "Manages financial operations",
                DepartmentEmail = "finance@company.com"
            };

            // Act
            var department = mapper.Map<Department>(viewModel);

            // Assert
            Assert.Equal(viewModel.DepartmentName, department.DepartmentName);
            Assert.Equal(viewModel.DepartmentDescription, department.DepartmentDescription);
            Assert.Equal(viewModel.DepartmentEmail, department.DepartmentEmail);
        }
    }
}

