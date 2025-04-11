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
        public static readonly List<String> LeaveType = new List<string>
        {
            "Sick",
            "Casual",
            "Life Events"
        };
        public static readonly List<String> LeaveStatus = new List<string>
        {
            "Approved",
            "Pending",
            "Rejected"
        };
    }
}
