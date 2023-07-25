using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs.Request
{
    public class JobTitleReqDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
