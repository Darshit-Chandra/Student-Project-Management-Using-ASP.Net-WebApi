using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.TaskStatusDTo
{
    public class TAskStatusGetDTO
    {
        [Required]
        public int TaskStatusID { get; set; }

        [Required]
        [MaxLength(20)]
        public string TaskStatusName { get; set; }

        [Required]
        [MaxLength(100)]
        public string TaskStatusCssClass { get; set; }
    }
}
