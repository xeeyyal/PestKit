namespace PestKitAB104.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}
