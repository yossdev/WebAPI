using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs.Request
{
    public class EmployeeReqDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int JobPositionId { get; set; }
        [Required]
        public int JobTitleId { get; set; }
    }
}
