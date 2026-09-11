using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.UserRoleDTO
{
    public class UserRoleGetDTO
    {
        [Required]
        public int RolePermissionID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}
