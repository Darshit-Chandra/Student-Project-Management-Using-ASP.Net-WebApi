using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.UserRoleDTO
{
    public class UserRolePostDTO
    {

        [Required]
        public int RoleID { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}
