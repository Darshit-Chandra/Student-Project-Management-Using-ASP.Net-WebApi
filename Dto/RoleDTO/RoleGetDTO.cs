using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.RoleDTO
{
    public class RoleGetDTO
    {
        [Required]
       public int RoleID { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
