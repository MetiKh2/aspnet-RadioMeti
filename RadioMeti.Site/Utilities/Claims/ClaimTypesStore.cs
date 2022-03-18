using System.Security.Claims;

namespace RadioMeti.Site.Utilities.Claims
{
    public static class ClaimStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim(ClaimTypesStore.EmployeeList,true.ToString()),
            new Claim(ClaimTypesStore.EmployeeDetails,true.ToString()),
            new Claim(ClaimTypesStore.EmployeeEdit,true.ToString()),
            new Claim(ClaimTypesStore.AddEmployee,true.ToString())
        };
    }

    public static class ClaimTypesStore
    {
        public const string EmployeeList = "EmployeeList";
        public const string EmployeeDetails = "EmployeeDetails";
        public const string EmployeeEdit = "EmployeeEdit";
        public const string AddEmployee = "AddEmployee";
    }
}
