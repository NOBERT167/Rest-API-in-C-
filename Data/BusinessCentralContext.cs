using Microsoft.EntityFrameworkCore;
using BusinessCentralApi.Models;

namespace BusinessCentralApi.Data
{
    public class BusinessCentralContext : DbContext
    {
        public BusinessCentralContext(DbContextOptions<BusinessCentralContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public class BusinessCentralContextFactory : Microsoft.EntityFrameworkCore.Design.IDesignTimeDbContextFactory<BusinessCentralContext>
        {
            public BusinessCentralContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BusinessCentralContext>();
                optionsBuilder.UseSqlServer("Server=localhost;Database=AnotherDb;Integrated Security=True;Encrypt=False;");

                return new BusinessCentralContext(optionsBuilder.Options);
            }
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeNo = "EMP001",
                    FirstName = "John",
                    LastName = "Doe",
                    JobTitle = "Sales Manager",
                    Department = "Sales",
                    Email = "john.doe@company.com",
                    PhoneNumber = "555-1234",
                    IsActive = true
                },
                new Employee
                {
                    EmployeeNo = "EMP002",
                    FirstName = "Jane",
                    LastName = "Smith",
                    JobTitle = "HR Specialist",
                    Department = "Human Resources",
                    Email = "jane.smith@company.com",
                    PhoneNumber = "555-5678",
                    IsActive = true
                }
            );
        }
    }
}
