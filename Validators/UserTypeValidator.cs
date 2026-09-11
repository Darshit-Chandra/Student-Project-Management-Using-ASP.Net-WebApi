using FluentValidation;
using SPMBACKENDSELF.Dto.UserTypeDTO;

namespace SPMBACKENDSELF.Validators
{
    public class UserTypeValidator:AbstractValidator<UserTypePostDTO>
    {
     public UserTypeValidator() {

            RuleFor(x => x.UserTypeName).NotEmpty()
                                        .WithMessage("User Type Name is required.")

                                        .MaximumLength(50)
                                        .WithMessage("User Type name cannot excced 50 characters");

            RuleFor(x => x.Description).MaximumLength(250)
                                       .WithMessage("Description cannot excced 250 characters");

        }
    }
}
