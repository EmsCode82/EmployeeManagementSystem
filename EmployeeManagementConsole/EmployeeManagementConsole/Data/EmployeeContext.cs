// Data/EmployeeContext.cs
using Microsoft.EntityFrameworkCore;
using EmployeeManagementConsole.Models;

namespace EmployeeManagementConsole.Data
{
    // DbContext to interact with dbEmployeeManager database
    public class EmployeeContext : DbContext
    {
        // DbSet for TEmployees table
        public DbSet<Employee> TEmployees { get; set; }
        // DbSet for TDepartments table
        public DbSet<Department> TDepartments { get; set; }

        // Configure connection to SQL Server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use the updated connection string
            optionsBuilder.UseSqlServer("Server=localhost;Database=dbEmployeeManager;Trusted_Connection=True;TrustServerCertificate=True;")
                         .EnableSensitiveDataLogging() // Enable detailed logging
                         .EnableDetailedErrors();      // Include detailed errors
        }

        // Map C# property names to database column names and define primary keys
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Employee entity
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.IntEmployeeID); // Primary key
            modelBuilder.Entity<Employee>()
                .Property(e => e.IntEmployeeID).HasColumnName("intEmployeeID");
            modelBuilder.Entity<Employee>()
                .Property(e => e.StrFirstName).HasColumnName("strFirstName");
            modelBuilder.Entity<Employee>()
                .Property(e => e.StrLastName).HasColumnName("strLastName");
            modelBuilder.Entity<Employee>()
                .Property(e => e.StrEmail).HasColumnName("strEmail");
            modelBuilder.Entity<Employee>()
                .Property(e => e.DblSalary).HasColumnName("dblSalary");
            modelBuilder.Entity<Employee>()
                .Property(e => e.DtmHireDate).HasColumnName("dtmHireDate");
            modelBuilder.Entity<Employee>()
                .Property(e => e.IntDepartmentID).HasColumnName("intDepartmentID");

            // Configure Department entity
            modelBuilder.Entity<Department>()
                .HasKey(d => d.IntDepartmentID); // Primary key
            modelBuilder.Entity<Department>()
                .Property(d => d.IntDepartmentID).HasColumnName("intDepartmentID");
            modelBuilder.Entity<Department>()
                .Property(d => d.StrDepartmentName).HasColumnName("strDepartmentName");

            // Configure relationship (fix for 'DepartmentIntDepartmentID' error)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)              // Employee has one Department
                .WithMany(d => d.Employees)             // Department has many Employees
                .HasForeignKey(e => e.IntDepartmentID); // Foreign key is IntDepartmentID
        }
    }
}