using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.TaskStatusDTo
{
    public class TaskStatusPostDTO
    {
        [Required]
        [MaxLength(20)]
        public string TaskStatusName { get; set; }

        [Required]
        [MaxLength(100)]
        public string TaskStatusCssClass { get; set; }
    }
}
