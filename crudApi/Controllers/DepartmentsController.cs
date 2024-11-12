using crudApi.Data;
using crudApi.DTOs.Departments;
using crudApi.DTOs.Employees;
using crudApi.Migrations;
using crudApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public DepartmentsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {

            var departments = context.Departments.Select(
                x => new GetDepartmentDTO()
                {
                    Id = x.Id,
                    Name = x.Name,

                }
                );

            return Ok(departments);
        }
        [HttpGet("Detials")]
        public IActionResult GetById(int Id)
        {
            var department = context.Departments
                             .Include(d => d.Employees) // Include related employees
                             .FirstOrDefault(d => d.Id == Id);

            if (department is null)
            {
                return NotFound("Department not found");
            }

            // Step 2: Map department and employee data to the DTO
            var departmentDTO = new OneDepartmentDTO()
            {
                Id = department.Id,
                Name = department.Name,
                Employees = department.Employees
                                      .Select(e => new GetEmployeeDTO()
                                      {
                                          Id = e.Id,
                                          Name = e.Name
                                      }).ToList()
            };

            return Ok(departmentDTO);
        }
        [HttpPost("Create")]
        public IActionResult Create(CreateDepartmentDTO depdto)
        {
            var department = new Department()
            {
                Name = depdto.Name
            };
            context.Departments.Add(department);
            context.SaveChanges();
            return Ok("department is added");
        }
        [HttpPut("Update")]
        public IActionResult Update(int Id, UpdateDepartmentDTO depDto)
        {
            var current = context.Departments.Find(Id);
            if (current is null)
            {
                return NotFound();
            }
            var departmente = new Department()
            {
                Name = depDto.Name

            };
            current.Name = departmente.Name;
            context.SaveChanges();
            return Ok("departmente updated successfully");
        }
        [HttpDelete("Remove")]
        public IActionResult Remove(int Id)
        {
            var current = context.Departments.Find(Id);
            if (current is null)
            {
                return NotFound();
            }
            context.Departments.Remove(current);
            context.SaveChanges();
            return Ok("Departmente deleted successfully");
        }
    }
}
