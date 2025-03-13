using Microsoft.AspNetCore.Http;
using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;


namespace Payroll.Application.Services.ServiceImplementation
{
    public class EmployeeService:IEmployeeService
    {
        private readonly IUnitOfWork unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Create(Employee employee, IFormFile file)
        {
            if (file != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "Images");
                Directory.CreateDirectory(uploadsFolder); 

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                employee.EmployeeImage = Path.Combine("Images", fileName);
            }

            unitOfWork.empRepository.Add(employee);
            await unitOfWork.SaveAsync();
        }

     

        public async Task Delete(int id)
        {
            var emp = await unitOfWork.empRepository.GetAsync(x => x.Id == id);
            unitOfWork.empRepository.Remove(emp);
            await unitOfWork.SaveAsync();

        }

        public async Task<Employee> GetById(int id)
        {
            return await unitOfWork.empRepository.GetAsync(x => x.Id==id);
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await unitOfWork.empRepository.GetAllAsync();
        }

        
            public async Task Update(Employee employee, IFormFile file)
        {
            var existingEmployee = await unitOfWork.empRepository.GetAsync(x => x.Id == employee.Id);
            if (existingEmployee == null) return;

            if (file != null)
            {
                if (!string.IsNullOrEmpty(existingEmployee.EmployeeImage))
                {
                    var oldFilePath = Path.Combine("wwwroot", existingEmployee.EmployeeImage);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                var uploadsFolder = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                existingEmployee.EmployeeImage = Path.Combine("uploads", fileName);
            }
            existingEmployee.department.DepartmentId = employee.department.DepartmentId;
            existingEmployee.Designation = employee.Designation;
            unitOfWork.empRepository.Update(existingEmployee);
            await unitOfWork.SaveAsync();
        }

    }
}

