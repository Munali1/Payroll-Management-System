
using Payroll.Application.Interfaces;
using Payroll.Domain.Entities;
using Payroll.Infrastructure.Data;



namespace Payroll.Infrastructure.Repository
{
    public class BankDetailsRepository : Repository<BankDetails>, IBankDetailsRepository
    {
        private readonly AppDbContext context;

        public BankDetailsRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(BankDetails bankDetails)
        {
           context.Banks.Update(bankDetails);
        }
    }
}
