namespace WebAPI.Models
{
    public class JobTitle
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<JobPosition> JobPositions { get; set; }
    }
}
