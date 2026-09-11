using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SPMBACKENDSELF.Models
{
    public class RoleModel
    {
        [Key]

        public int RoleID { get; set; }
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
        [JsonIgnore]

        public ICollection<UserRoleModel> UserRoleModels { get; set; } = new List<UserRoleModel>();

    }
}
