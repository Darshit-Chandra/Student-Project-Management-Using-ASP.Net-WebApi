using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.ProjectMasterDTO
{
    public class ProjectMasterGetDTO
    {
        [Required]
        public int ProjectID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectTitle { get; set; }

        public string? Description { get; set; }
    }
}
