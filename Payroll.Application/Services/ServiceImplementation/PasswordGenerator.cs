using Payroll.Application.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceImplementation
{
    public class PasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword(int length = 10)
        {
            if (length < 6) throw new ArgumentException("Password length must be at least 6.");

            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "@#$!";
            const string all = upper + lower + digits + special;

            var random = new Random();
            var password = new StringBuilder();

            // Ensure at least one character from each required category
            password.Append(upper[random.Next(upper.Length)]);
            password.Append(lower[random.Next(lower.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(special[random.Next(special.Length)]);

            // Fill the rest with random chars
            for (int i = 4; i < length; i++)
            {
                password.Append(all[random.Next(all.Length)]);
            }

            return new string(password.ToString().OrderBy(x => random.Next()).ToArray());
        }

    }
}
