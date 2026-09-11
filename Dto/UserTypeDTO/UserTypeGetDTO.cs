using System.ComponentModel.DataAnnotations;

namespace SPMBACKENDSELF.Dto.UserTypeDTO
{
    public class UserTypeGetDTO
    {
        [Required]
        public int UserTypeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserTypeName { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
