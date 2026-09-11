using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPMBACKENDSELF.Dto.TaskDTO
{
    public class TaskPostDTO
    {
        [Required]
        public int ProjectAllocationID { get; set; }

        [Required]
        [MaxLength(200)]
        public string TaskTitle { get; set; }

        public string? TaskDescription { get; set; }

        [Required]
        public int TaskStatusID { get; set; }

        [Required]
        public int TaskPriorityID { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal AssignedScore { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? EarnedScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; }

        [Required]
        public DateTime TaskAssignedDate { get; set; }

        public DateTime? TaskStartDate { get; set; }

        public DateTime? TaskDueDate { get; set; }

        public DateTime? TaskCompletedDate { get; set; }

        public DateTime? NextFollowUpDate { get; set; }

        [MaxLength(500)]
        public string? FacultyRemarks { get; set; }

        [MaxLength(500)]
        public string? StudentRemarks { get; set; }
    }
}
