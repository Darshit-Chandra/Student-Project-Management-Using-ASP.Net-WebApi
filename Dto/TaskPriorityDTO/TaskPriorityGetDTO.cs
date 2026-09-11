using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.TaskPriorityDTO
{
    public class TaskPriorityGetDTO
    {
        [Required]
        public int TaskPriorityID { get; set; }

        [Required]
        [MaxLength(20)]
        public string TaskPriorityName { get; set; }

        [Required]
        [MaxLength(20)]
        public string TaskPriortyCssClass { get; set; }
    }
}
