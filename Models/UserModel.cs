using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SPMBACKENDSELF.Models
{
    public class UserModel
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public int UserTypeID { get; set; }

        [ForeignKey(nameof(UserTypeID))]
        public UserTypeModel UserType { get; set; } = null!;


        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? UserCode { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        public string ProfilePicturePath { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }


        [JsonIgnore]
        public ICollection<UserRoleModel> UserRoleModels { get; set; }
            = new List<UserRoleModel>();


        [JsonIgnore]
        [InverseProperty(nameof(ProjectAllocationModel.Student))]
        public ICollection<ProjectAllocationModel> StudentProjectAllocations { get; set; }
            = new List<ProjectAllocationModel>();


        [JsonIgnore]
        [InverseProperty(nameof(ProjectAllocationModel.Faculty))]
        public ICollection<ProjectAllocationModel> FacultyProjectAllocations { get; set; }
            = new List<ProjectAllocationModel>();
    }
}