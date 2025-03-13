using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceInterface
{
    public interface IBankDetailsService
    {

        Task Create(BankDetails bankDetails);
        Task Delete(int id);
        Task<BankDetails> GetById(int id);
        Task<List<BankDetails>> GetBankList();
        Task Update(BankDetails bankDetails);
    }
}
