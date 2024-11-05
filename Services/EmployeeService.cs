using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessCentralApi.Data;
using BusinessCentralApi.Models;
using BusinessCentralApi.Dtos;

namespace BusinessCentralApi.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> QueryEmployeesAsync(EmployeeQueryDto queryParams);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly BusinessCentralContext _context;

        public EmployeeService(BusinessCentralContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> QueryEmployeesAsync(EmployeeQueryDto queryParams)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.EmployeeNo))
                query = query.Where(e => e.EmployeeNo.Contains(queryParams.EmployeeNo));

            if (!string.IsNullOrEmpty(queryParams.FirstName))
                query = query.Where(e => e.FirstName.Contains(queryParams.FirstName));

            if (!string.IsNullOrEmpty(queryParams.LastName))
                query = query.Where(e => e.LastName.Contains(queryParams.LastName));

            if (!string.IsNullOrEmpty(queryParams.JobTitle))
                query = query.Where(e => e.JobTitle.Contains(queryParams.JobTitle));

            if (!string.IsNullOrEmpty(queryParams.Department))
                query = query.Where(e => e.Department.Contains(queryParams.Department));

            return await query.ToListAsync();
        }
    }
}