using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPMBACKENDSELF.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskID { get; set; }
        [Required]
        public int ProjectAllocationID { get; set; }
        [ForeignKey(nameof(ProjectAllocationID))]
        public ProjectAllocationModel? ProjectAllocation { get; set; }

        [Required]
        [MaxLength(200)]
        public string TaskTitle { get; set; }
        public string? TaskDescription { get; set; }
        [Required]
        public int TaskStatusID { get; set; }
        [ForeignKey(nameof(TaskStatusID))]
        public TaskStatusModel? TaskStatus { get; set; }

        [Required]
        
        public int TaskPriorityID { get; set; }
        [ForeignKey(nameof(TaskPriorityID))]
        public TaskPriorityModel? taskPriority { get; set; }
        [Required]
        public decimal AssignedScore { get; set; }
        public decimal? EarnedScore { get; set; }
        [Required]
        public double ProgressPercentage { get; set; }
        [Required]
        public DateTime TaskAssignedDate { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public DateTime? TaskSCompleteDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        [MaxLength(500)]
        public string? FacultyRemarks { get; set; }
        [MaxLength(500)]
        public string? StudeStudentRemarks { get; set; }

    }
}
