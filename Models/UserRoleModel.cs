using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPMBACKENDSELF.Models
{
    public class UserRoleModel
    {
        [Key]
        public int RolePermissionID { get; set; }
        [Required]
       
        public int RoleID { get; set; }
        [ForeignKey(nameof(RoleID))]
        public RoleModel Role { get; set; }

        [Required]
        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public UserModel User { get; set; }
    }
}
