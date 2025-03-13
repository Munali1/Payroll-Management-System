
using Payroll.Domain.Entities;


namespace Payroll.Application.Interfaces
{
    public interface IBankDetailsRepository:IRepository<BankDetails>
    {
        void Update(BankDetails bankDetails);
    }
}
