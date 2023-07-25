namespace WebAPI.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string NIK { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public JobPosition JobPosition { get; set; }
        public JobTitle JobTitle { get; set; }
    }
}
