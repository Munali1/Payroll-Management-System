using Microsoft.VisualBasic;
using Payroll.Application.Interfaces;
using Payroll.Application.Services.ServiceInterface;
using Payroll.Domain.Entities;


namespace Payroll.Application.Services.ServiceImplementation
{
    public class BankDetailsService : IBankDetailsService
    {
        private readonly IUnitOfWork unitOfWork;

        public BankDetailsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Create(BankDetails bankDetails)
        {   
            unitOfWork.bankDetailsRepository.Add(bankDetails);
            await unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            var dep = await unitOfWork.bankDetailsRepository.GetAsync(x => x.Id == id);
            unitOfWork.bankDetailsRepository.Remove(dep);
            await unitOfWork.SaveAsync();
        }

        public async Task<List<BankDetails>> GetBankList()
        {
            var dep = await unitOfWork.bankDetailsRepository.GetAllAsync(null,"Employee");
            return dep;
        }

        public async Task<BankDetails> GetById(int id)
        {
            return (await unitOfWork.bankDetailsRepository.GetAsync(x => x.Id == id,"Employee"));
        }

        public async Task Update(BankDetails bankDetails)
        {
            unitOfWork.bankDetailsRepository.Update(bankDetails);
            await unitOfWork.SaveAsync();
        }
    }
}
