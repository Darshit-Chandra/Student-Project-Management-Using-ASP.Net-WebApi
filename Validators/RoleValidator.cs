using FluentValidation;
using SPMBACKENDSELF.Dto.RoleDTO;

namespace SPMBACKENDSELF.Validators
{
    public class RoleValidator:AbstractValidator<RolePostDTO>
    {
        public RoleValidator() {
            RuleFor(x => x.RoleName).NotEmpty()
                                        .WithMessage("Role Name is required.")

                                        .MaximumLength(50)
                                        .WithMessage("Role Name cannot exceed 50 character");

            RuleFor(x => x.Description).MaximumLength(250)
                                       .WithMessage("Description cannot exceed 250 character");

        }
    }
}
