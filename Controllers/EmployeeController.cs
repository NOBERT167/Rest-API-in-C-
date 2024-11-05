using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessCentralApi.Services;
using BusinessCentralApi.Models;
using BusinessCentralApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace BusinessCentralApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("query")]
        public async Task<ActionResult<ApiResponse<List<Employee>>>> QueryEmployees([FromBody] EmployeeQueryDto queryParams)
        {
            try
            {
                var employees = await _employeeService.QueryEmployeesAsync(queryParams);

                return Ok(new ApiResponse<List<Employee>>
                {
                    Status = "success",
                    Count = employees.Count,
                    Data = employees,
                    Message = "Employees retrieved successfully.",
                    ErrorDetails = ""
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<Employee>?>
                {
                    Status = "error",
                    Message = "An error occurred while processing your request.",
                    ErrorDetails = ex.Message,
                    Data = null
                });
            }
        }
    }
}