using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementConsole.Models
{    
    public class Employee
    {
        public int IntEmployeeID { get; set; } // Primary key, matches intEmployeeID
        public string StrFirstName { get; set; } // First name, VARCHAR(255)
        public string StrLastName { get; set; } // Last name, VARCHAR(255)
        public string StrEmail { get; set; } // Email, VARCHAR(255)
        public decimal DblSalary { get; set; } // Salary, DECIMAL(10,2)
        public DateTime DtmHireDate { get; set; } // Hire date, DATE
        public int IntDepartmentID { get; set; } // Foreign key to TDepartments
        public Department Department { get; set; } // Navigation property for EF
    }
}
