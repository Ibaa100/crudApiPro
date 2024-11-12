namespace crudApi.DTOs.Employees
{
    public class UpdateEmployeeDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
    }
}
