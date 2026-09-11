using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.ProjectMasterDTO
{
    public class ProjectMasterPostDTO
    {
        [Required]
        [MaxLength(200)]
        public string ProjectTitle { get; set; }

        public string? Description { get; set; }
    }
}
