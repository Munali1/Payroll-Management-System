using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceInterface
{
    public interface IEmailServiceInterface
    {
        void sendEmail(string email, string body, string subject);
    }
}
