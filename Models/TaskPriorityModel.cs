using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SPMBACKENDSELF.Models
{
    public class TaskPriorityModel
    {
        [Key]
        public int TaskPriorityID { get; set; }
        [Required]
        [MaxLength(20)]
        public string TaskPriorityName { get; set; }
        [Required]
        [MaxLength(20)]

        public string TaskPriortyCssClass { get; set; }
        [JsonIgnore]
        public ICollection<TaskModel> TaskPriorityIDs { get; set; } = new List<TaskModel>();

    }
}
