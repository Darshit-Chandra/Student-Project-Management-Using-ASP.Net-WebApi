using FluentValidation;
using SPMBACKENDSELF.Dto.TaskPriorityDTO;

namespace SPMBACKENDSELF.Validators
{
    public class TaskPriorityValidator:AbstractValidator<TaskPriorityPostDTO>
    {
        public TaskPriorityValidator()
        {
            RuleFor(x => x.TaskPriorityName).NotEmpty()
                                    .WithMessage("Task Priority Name is required.")

                                    .MaximumLength(20)
                                    .WithMessage("Task Priority Name cannot exceed 20 character");

            RuleFor(x => x.TaskPriortyCssClass).MaximumLength(20)
                                       .WithMessage("Task Priority Css Class cannot exceed 20 character");                              
        }
    }
}
