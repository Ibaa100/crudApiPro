using crudApi.Data;
using crudApi.DTOs.Departments;
using crudApi.DTOs.Employees;
using crudApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public EmployeesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var employees = context.Employees.Select(
                x => new GetEmployeeDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }
                );
            return Ok(employees);
        }
        [HttpGet("Detials")]
        public IActionResult GetById(int Id)
        {
            var employee = context.Employees.Find(Id);
            if (employee is null)
            {
                return NotFound("employee not found");
            }
            var department = context.Departments.Find(employee.DepartmentId);

            // Step 3: Map employee and department data to the DTO
            var employeeDTO = new OneEmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                Description = employee.Description,
                DepartmentId = employee.DepartmentId,
                DepartmentName = department?.Name
            };
            return Ok(employeeDTO);
        }
        [HttpPost("Create")]
        public IActionResult Create(CreateEmployeeDTO employeedto)
        {
            var employee = new Employee()
            {
                Name = employeedto.Name,
                Description =employeedto.Description,
                DepartmentId =employeedto.DepartmentId,
            };
            context.Employees.Add(employee);
            context.SaveChanges();
            return Ok("employee is added");
        }
        [HttpPut("Update")]
        public IActionResult Update(int Id, UpdateEmployeeDTO employeedto)
        {
            var current = context.Employees.Find(Id);
            if (current is null)
            {
                return NotFound();
            }

            var employee = new Employee()
            {
                Name = employeedto.Name,
                Description = employeedto.Description,
                DepartmentId = employeedto.DepartmentId,

            };
            current.Name = employee.Name;
            current.Description = employee.Description;
            current.DepartmentId = employee.DepartmentId;
            context.SaveChanges();
            return Ok("employee updated successfully");
        }
        [HttpDelete("Remove")]
        public IActionResult Remove(int Id)
        {
            var current = context.Employees.Find(Id);
            if (current is null)
            {
                return NotFound();
            }
            context.Employees.Remove(current);
            context.SaveChanges();
            return Ok("employee deleted successfully");
        }
    }
}
