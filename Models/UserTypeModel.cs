using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SPMBACKENDSELF.Models
{
    public class UserTypeModel
    {
        [Key]
        public int UserTypeID { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserTypeName { get;set; }
        [MaxLength(250)]

        public string? Description { get; set; }
        [JsonIgnore]
        public ICollection<UserModel> Users { get; set; } = new List<UserModel>();

    }
}
