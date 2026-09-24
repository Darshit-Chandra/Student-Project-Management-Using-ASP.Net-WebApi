using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto.ProjectAllocationDTO;
using SPMBACKENDSELF.Dto.TaskPriorityDTO;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TaskPriorityController : ControllerBase
    {
        private readonly AppDbContext _Context;
        private readonly IValidator<TaskPriorityPostDTO> _taskPriorityValidator;    
        public TaskPriorityController(AppDbContext context, IValidator<TaskPriorityPostDTO> taskPriorityValidator)
        {
            _Context = context;
            _taskPriorityValidator = taskPriorityValidator;
        }

        // GET: api/TaskPriority
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]

        public async Task<IActionResult> GetAllTaskPriority()
        {
            var taskPriority = await _Context.TaskPriority
                .Select(t => new TaskPriorityGetDTO
                {
                    TaskPriorityID = t.TaskPriorityID,
                    TaskPriorityName = t.TaskPriorityName,
                    TaskPriortyCssClass = t.TaskPriortyCssClass
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<TaskPriorityGetDTO>>
            {
                Success = true,
                Message = "TaskPriority Retrieved Successfully",
                Data = taskPriority
            });
        }

        // GET: api/TaskPriority/1
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Faculty")]

        public async Task<IActionResult> GetById(int id)
        {
            var taskPriority = await _Context.TaskPriority
                .Where(t => t.TaskPriorityID == id)
                .Select(t => new TaskPriorityGetDTO
                {
                    TaskPriorityID = t.TaskPriorityID,
                    TaskPriorityName = t.TaskPriorityName,
                    TaskPriortyCssClass = t.TaskPriortyCssClass
                })
                .FirstOrDefaultAsync();

            if (taskPriority == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the TaskPriority with id : {id}"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"TaskPriority with {id} Is Retrieved Successfully",
                Data = taskPriority
            });
        }

        // POST: api/TaskPriority
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> PostTaskPriority(
            TaskPriorityPostDTO taskPriorityDto)
        {
            var taskPriority = new TaskPriorityModel
            {
                TaskPriorityName = taskPriorityDto.TaskPriorityName,
                TaskPriortyCssClass = taskPriorityDto.TaskPriortyCssClass
            };

            _Context.TaskPriority.Add(taskPriority);

            await _Context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task Priority Added Successfully",
                Data = taskPriority
            });
        }

        // PUT: api/TaskPriority/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateTaskPriority(
            int id,
            TaskPriorityPostDTO taskPriorityDto)
        {
            var oldTaskPriority = await _Context.TaskPriority.FindAsync(id);

            if (oldTaskPriority == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Taskpriority with id : {id}"
                });
            }

            oldTaskPriority.TaskPriorityName = taskPriorityDto.TaskPriorityName;
            oldTaskPriority.TaskPriortyCssClass = taskPriorityDto.TaskPriortyCssClass;

            await _Context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"Task With {id} Is Updated Successfully",
                Data = oldTaskPriority
            });
        }

        // DELETE: api/TaskPriority/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteTaskPriority(int id)
        {
            var taskPriority = await _Context.TaskPriority.FindAsync(id);

            if (taskPriority == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Task with id : {id}"
                });
            }

            _Context.TaskPriority.Remove(taskPriority);

            await _Context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "TaskPriority Deleted Successfully",
               
            });
        }
    }
}