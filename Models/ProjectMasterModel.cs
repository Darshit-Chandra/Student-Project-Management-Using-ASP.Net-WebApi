using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SPMBACKENDSELF.Models
{
    public class ProjectMasterModel
    {
        [Key]
        public int ProjectID { get; set; }
        [Required]
        [MaxLength(200)]
        public string ProjectTitle { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public ICollection<ProjectAllocationModel> projectAllocations { get; set; }    = new List<ProjectAllocationModel>();

    }
}
