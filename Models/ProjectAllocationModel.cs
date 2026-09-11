using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPMBACKENDSELF.Models
{
    public class ProjectAllocationModel
    {
        [Key]
        public int ProjectAllocationID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [ForeignKey(nameof(ProjectID))]
        public ProjectMasterModel Project { get; set; } = null!;


        [Required]
        public int StudentID { get; set; }

        [ForeignKey(nameof(StudentID))]
        [InverseProperty(nameof(UserModel.StudentProjectAllocations))]
        public UserModel Student { get; set; } = null!;


        [Required]
        public int FacultyID { get; set; }

        [ForeignKey(nameof(FacultyID))]
        [InverseProperty(nameof(UserModel.FacultyProjectAllocations))]
        public UserModel Faculty { get; set; } = null!;


        [Required]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

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


        public ICollection<TaskModel> Tasks { get; set; }
            = new List<TaskModel>();
    }
}