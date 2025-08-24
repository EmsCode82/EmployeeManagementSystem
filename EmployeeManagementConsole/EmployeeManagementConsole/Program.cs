using EmployeeManagementConsole.Data;
using EmployeeManagementConsole.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EmployeeManagementConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create DbContext instance to interact with dbEmployeeManager
            using (var context = new EmployeeContext())
            {
                // Main menu loop
                while (true)
                {
                    Console.WriteLine("\nEmployee Management System");
                    Console.WriteLine("1. Add Employee");
                    Console.WriteLine("2. View All Employees");
                    Console.WriteLine("3. Update Employee");
                    Console.WriteLine("4. Delete Employee");
                    Console.WriteLine("5. Exit");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine();

                    if (choice == "5") break;

                    switch (choice)
                    {
                        case "1":
                            AddEmployee(context);
                            break;
                        case "2":
                            ViewEmployees(context);
                            break;
                        case "3":
                            UpdateEmployee(context);
                            break;
                        case "4":
                            DeleteEmployee(context);
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
        }

        // Add a new employee to TEmployees
        static void AddEmployee(EmployeeContext context)
        {
            try
            {
                Console.Write("First Name: ");
                string firstName = Console.ReadLine();
                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Salary (e.g., 50000.00): ");
                decimal salary = decimal.Parse(Console.ReadLine());
                Console.Write("Hire Date (YYYY-MM-DD): ");
                DateTime hireDate = DateTime.Parse(Console.ReadLine());
                Console.Write("Department ID (1=HR, 2=IT, 3=Finance): ");
                int deptId = int.Parse(Console.ReadLine());

                // Create new Employee object
                var employee = new Employee
                {
                    StrFirstName = firstName,
                    StrLastName = lastName,
                    StrEmail = email,
                    DblSalary = salary,
                    DtmHireDate = hireDate,
                    IntDepartmentID = deptId
                };

                // Add to database and save
                context.TEmployees.Add(employee);
                context.SaveChanges();
                Console.WriteLine("Employee added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding employee: {ex.Message}");
            }
        }

        // View all employees with department names
        static void ViewEmployees(EmployeeContext context)
        {
            try
            {
                // Use LINQ to join TEmployees and TDepartments
                var employees = context.TEmployees
                    .Join(context.TDepartments,
                        e => e.IntDepartmentID,
                        d => d.IntDepartmentID,
                        (e, d) => new
                        {
                            e.IntEmployeeID,
                            e.StrFirstName,
                            e.StrLastName,
                            e.StrEmail,
                            e.DblSalary,
                            e.DtmHireDate,
                            DepartmentName = d.StrDepartmentName
                        })
                    .ToList();

                // Display results
                if (employees.Count == 0)
                {
                    Console.WriteLine("No employees found.");
                    return;
                }

                foreach (var emp in employees)
                {
                    Console.WriteLine($"ID: {emp.IntEmployeeID}, Name: {emp.StrFirstName} {emp.StrLastName}, " +
                        $"Email: {emp.StrEmail}, Salary: {emp.DblSalary}, Hire Date: {emp.DtmHireDate.ToShortDateString()}, " +
                        $"Dept: {emp.DepartmentName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error viewing employees: {ex.Message}");
            }
        }

        // Update an existing employee
        static void UpdateEmployee(EmployeeContext context)
        {
            try
            {
                // Prompt for Employee ID
                Console.Write("Enter Employee ID to update: ");
                int id = int.Parse(Console.ReadLine());

                // Find the employee by ID
                var employee = context.TEmployees.Find(id);
                if (employee == null)
                {
                    Console.WriteLine("Employee not found.");
                    return;
                }

                // Display current employee details for reference
                Console.WriteLine($"\nCurrent Details for Employee ID {id}:");
                Console.WriteLine($"First Name: {employee.StrFirstName}");
                Console.WriteLine($"Last Name: {employee.StrLastName}");
                Console.WriteLine($"Email: {employee.StrEmail}");
                Console.WriteLine($"Salary: {employee.DblSalary}");
                Console.WriteLine($"Hire Date: {employee.DtmHireDate.ToShortDateString()}");
                Console.WriteLine($"Department ID: {employee.IntDepartmentID}");

                // Prompt for updates, allowing blank input to keep current values
                Console.Write("\nNew First Name (leave blank to keep current): ");
                string firstName = Console.ReadLine();
                if (!string.IsNullOrEmpty(firstName))
                    employee.StrFirstName = firstName;

                Console.Write("New Last Name (leave blank to keep current): ");
                string lastName = Console.ReadLine();
                if (!string.IsNullOrEmpty(lastName))
                    employee.StrLastName = lastName;

                Console.Write("New Email (leave blank to keep current): ");
                string email = Console.ReadLine();
                if (!string.IsNullOrEmpty(email))
                {
                    // Check for email uniqueness
                    if (context.TEmployees.Any(e => e.StrEmail == email && e.IntEmployeeID != id))
                    {
                        Console.WriteLine("Error: Email already exists.");
                        return;
                    }
                    employee.StrEmail = email;
                }

                Console.Write("New Salary (leave blank to keep current): ");
                string salaryInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(salaryInput))
                {
                    if (!decimal.TryParse(salaryInput, out decimal salary) || salary < 0)
                    {
                        Console.WriteLine("Error: Invalid salary format or negative value.");
                        return;
                    }
                    employee.DblSalary = salary;
                }

                Console.Write("New Hire Date (YYYY-MM-DD, leave blank to keep current): ");
                string hireDateInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(hireDateInput))
                {
                    if (!DateTime.TryParse(hireDateInput, out DateTime hireDate))
                    {
                        Console.WriteLine("Error: Invalid date format. Use YYYY-MM-DD.");
                        return;
                    }
                    employee.DtmHireDate = hireDate;
                }

                Console.Write("New Department ID (1=HR, 2=IT, 3=Finance, leave blank to keep current): ");
                string deptIdInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(deptIdInput))
                {
                    if (!int.TryParse(deptIdInput, out int deptId) || !context.TDepartments.Any(d => d.IntDepartmentID == deptId))
                    {
                        Console.WriteLine("Error: Invalid Department ID. Use 1 (HR), 2 (IT), or 3 (Finance).");
                        return;
                    }
                    employee.IntDepartmentID = deptId;
                }

                // Save changes to database
                context.SaveChanges();
                Console.WriteLine("Employee updated successfully.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Invalid input format. Please ensure numbers and dates are correctly formatted.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error updating employee: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Delete an employee
        static void DeleteEmployee(EmployeeContext context)
        {
            try
            {
                Console.Write("Enter Employee ID to delete: ");
                int id = int.Parse(Console.ReadLine());
                var employee = context.TEmployees.Find(id);
                if (employee == null)
                {
                    Console.WriteLine("Employee not found.");
                    return;
                }

                // Remove from database and save
                context.TEmployees.Remove(employee);
                context.SaveChanges();
                Console.WriteLine("Employee deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting employee: {ex.Message}");
            }
        }
    }
}