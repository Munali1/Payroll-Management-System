using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceInterface
{
    public interface IEmailServiceInterface
    {
        Task sendEmail(string email, string body, string subject,byte[] attachment=null, string attachmentName=null);
       
    }
}
