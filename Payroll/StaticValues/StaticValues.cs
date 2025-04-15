namespace Payroll.Web.StaticValues
{
    public class StaticValues
    {
        public static readonly List<string> Designation = new List<string>
        {
           "Intern",
          "Associate",
          "Mid Level",
          "Senior",
          "Principal"
        };
        public static readonly List<string> LeaveType = new List<string>
        {
            "Sick",
            "Casual",
            "Life Events"
        };
        public static readonly List<string> LeaveStatus = new List<string>
        {
            "Approved",
            "Pending",
            "Rejected"
        };
        public static int CountWeekendsLinq(DateTime start, DateTime end)
        {
            return Enumerable.Range(0, (end - start).Days + 1)
                             .Select(offset => start.AddDays(offset))
                             .Count(date => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        }
    }
}
