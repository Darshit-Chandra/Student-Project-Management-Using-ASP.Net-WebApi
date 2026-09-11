using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.RoleDTO
{
    public class RolePostDTO
    {
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
