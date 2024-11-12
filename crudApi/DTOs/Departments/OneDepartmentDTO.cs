using crudApi.DTOs.Employees;

namespace crudApi.DTOs.Departments
{
    public class OneDepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetEmployeeDTO> Employees { get; set; }
    }
}
