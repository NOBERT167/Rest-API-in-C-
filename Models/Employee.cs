using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCentralApi.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        [Column("EmployeeNo")]
        public required string EmployeeNo { get; set; }

        [Required]
        [Column("FirstName")]
        public required string FirstName { get; set; }

        [Required]
        [Column("LastName")]
        public required string LastName { get; set; }

        [Column("JobTitle")]
        public required string JobTitle { get; set; }

        [Column("Department")]
        public required string Department { get; set; }

        [Column("Email")]
        public required string Email { get; set; }

        [Column("PhoneNumber")]
        public required string PhoneNumber { get; set; }

        [Column("HireDate")]
        public DateTime? HireDate { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;
    }
}