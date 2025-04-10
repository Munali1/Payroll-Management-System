using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceInterface
{
     public interface IPasswordGenerator
    {
        string GeneratePassword(int length=10);
    }
}
