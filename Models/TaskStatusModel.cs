using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SPMBACKENDSELF.Models
{
    public class TaskStatusModel
    {
        [Key]
        public int TaskStatusID { get; set; }
        [Required]
        [MaxLength(20)]
        public string TaskStatusName { get; set; }
        [Required]
        [MaxLength(100)]
        public string TaskStatusCssClass { get; set; }
        [JsonIgnore]
        public ICollection<TaskModel> TaskStatusIDs { get; set; } = new List<TaskModel>();

    }
}
