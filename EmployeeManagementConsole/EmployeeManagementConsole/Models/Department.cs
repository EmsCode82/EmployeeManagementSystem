using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementConsole.Models
{    
    public class Department
    {
        public int IntDepartmentID { get; set; } // Primary key, matches intDepartmentID
        public string StrDepartmentName { get; set; } // Department name, VARCHAR(255)
        public List<Employee> Employees { get; set; } // Navigation property for EF (one-to-many)
    }
}
