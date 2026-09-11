using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPMBACKENDSELF.Dto.ProjectAllocationDTO
{
    public class ProjectAllocationGetDTO
    {
        [Required]
        public int ProjectAllocationID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [Required]
        public int FacultyID { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }

        [Required]
        public DateTime ProjectStartDate { get; set; }

        [Required]
        public DateTime ProjectEndDate { get; set; }

        [Required]
        public int TotalTasksGiven { get; set; }

        [Required]
        public int TotalCompletedTasks { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; }

        [MaxLength(1)]
        public string? OverAllGrade { get; set; }
    }
}
