﻿

using Payroll.Domain.Entities;
using System.Globalization;

namespace Payroll.Application.Interfaces
{
   public interface IEmployeeRepository:IRepository<Employee>
    {
        IEnumerable<Employee> GetEmployeesByDepartment(int departmentId);
        string getFullName(string id);

        Salary getSalaryDetails(int id);

        BankDetails GetBankDetails(int id);

        int GetEmployeeIdFromUserId(String id);
        void Update(Employee employee);
    }
}
