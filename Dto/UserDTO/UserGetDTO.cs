using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.UserDTO
{
    public class UserGetDTO
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int UserTypeID { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string? UserCode { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; }

        [Required]
        [MaxLength(500)]
        public string ProfilePicturePath { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
