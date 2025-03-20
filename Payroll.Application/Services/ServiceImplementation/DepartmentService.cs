using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceImplementation
{
   public class DepartmentService:IDepartmentService
    {
        private readonly IUnitOfWork unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
       public async Task Create(Department department)
        {
            unitOfWork.departmentRepository.Add(department);
            await unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
           var dep=await unitOfWork.departmentRepository.GetAsync(x=>x.DepartmentId==id);
            unitOfWork.departmentRepository.Remove(dep);
            await unitOfWork.SaveAsync();
        }

        public async Task<Department> GetById(int id)
        {
            return await unitOfWork.departmentRepository.GetAsync(x => x.DepartmentId == id);
        }

        public async Task<List<Department>> GetDepartments()
        {
            return await unitOfWork.departmentRepository.GetAllAsync();
        }

        public List<Employee> getEmployeeInDepartment(int id)
        {
            return unitOfWork.departmentRepository.getEmployeesInDepartment(id);
        }

        public async Task Update(Department department)
        {
            var dep = await unitOfWork.departmentRepository.GetAsync(x => x.DepartmentId == department.DepartmentId);
            unitOfWork.departmentRepository.Update(dep);
           await unitOfWork.SaveAsync();
        }
    }
}
