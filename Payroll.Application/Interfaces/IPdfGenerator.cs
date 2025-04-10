using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Interfaces
{
    public interface IPdfGenerator
    {
        byte[] GeneratePdfFromHtml(string html);
    }
}
