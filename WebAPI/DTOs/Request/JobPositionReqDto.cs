using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs.Request
{
    public class JobPositionReqDto
    {
        [Required]
        public int JobTitleId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
