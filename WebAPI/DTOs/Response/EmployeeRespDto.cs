namespace WebAPI.DTOs.Response
{
    public class EmployeeRespDto
    {
        public Guid Id { get; set; }
        public string NIK { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public JobPositionBaseRespDto JobPosition { get; set; }
        public JobTitleRespDto JobTitle { get; set; }
    }
}
