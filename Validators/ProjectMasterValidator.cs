using FluentValidation;
using SPMBACKENDSELF.Dto.ProjectMasterDTO;
namespace SPMBACKENDSELF.Validators
{
    public class ProjectMasterValidator : AbstractValidator<ProjectMasterPostDTO>
    {
        public ProjectMasterValidator()
        {
            RuleFor(x => x.ProjectTitle).NotEmpty()
                                         .WithMessage("Project Title is required.")
                                         .MaximumLength(100)
                                         .WithMessage("Project Title cannot exceed 100 characters.");
            RuleFor(x => x.Description).MaximumLength(500)
                                       .WithMessage("Description cannot exceed 500 characters.");
        }
        
    }
}
