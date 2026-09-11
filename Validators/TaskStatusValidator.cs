using FluentValidation;
using SPMBACKENDSELF.Dto.TaskStatusDTo;

namespace SPMBACKENDSELF.Validators
{
    public class TaskStatusValidator:AbstractValidator<TaskStatusPostDTO>
    {
        public TaskStatusValidator()
        {
            RuleFor(x => x.TaskStatusName).NotEmpty()
                                       .WithMessage("Task Status Name is required.")

                                       .MaximumLength(20)
                                       .WithMessage("Task Status Name cannot exceed 20 character");

            RuleFor(x => x.TaskStatusCssClass).MaximumLength(100)
                                       .WithMessage("Task Status Css Class cannot exceed 100 character");
                                                        
        }
    }
}
