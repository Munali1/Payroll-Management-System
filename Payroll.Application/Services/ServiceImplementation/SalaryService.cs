using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;

namespace Payroll.Application.Services.ServiceImplementation
{
    public class SalaryService : ISalaryService
    {
        private readonly IUnitOfWork unitOfWork;

        public SalaryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Create(Salary salary)
        {
            unitOfWork.salaryRepository.Add(salary);
            await unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var sal = await unitOfWork.salaryRepository.GetAsync(x => x.Id == id,"Employee");
            unitOfWork.salaryRepository.Remove(sal);
            await unitOfWork.SaveAsync();
        }

        public IEnumerable<Salary> GetAll()
        {
            var sal = unitOfWork.salaryRepository.GetSalary();
            return sal; 
        }

        public async Task<Salary> GetById(int id)
        {
            var sal = await unitOfWork.salaryRepository.GetAsync(x => x.Id == id,"Employee");
            return sal;
        }

        public async Task<List<Salary>> GetSalaryList()
        {
            return await unitOfWork.salaryRepository.GetAllAsync(null,"Employee");
        }

        public async Task Update(Salary salary)
        {
            unitOfWork.salaryRepository.Update(salary);
            await unitOfWork.SaveAsync();
        }
    }
}
