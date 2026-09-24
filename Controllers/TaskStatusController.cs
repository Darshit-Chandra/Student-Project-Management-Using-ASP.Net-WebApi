using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto.TaskPriorityDTO;
using SPMBACKENDSELF.Dto.TaskStatusDTo;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TaskStatusController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<TaskStatusPostDTO> _taskStatusValidator;

        public TaskStatusController(AppDbContext context, IValidator<TaskStatusPostDTO> taskStatusValidator)
        {
            _context = context;
            _taskStatusValidator = taskStatusValidator;
        }

        // GET: api/TaskStatus
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]

        public async Task<IActionResult> GetTaskStatus()
        {
            var taskStatus = await _context.TaskStatus
                .Select(t => new TAskStatusGetDTO
                {
                    TaskStatusID = t.TaskStatusID,
                    TaskStatusName = t.TaskStatusName,
                    TaskStatusCssClass = t.TaskStatusCssClass
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<TAskStatusGetDTO>>
            {
                Success = true,
                Message = "TaskStatus Retrieved Successfully",
                Data = taskStatus
            });
        }

        // GET: api/TaskStatus/1
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> GetTaskStatusByID(int id)
        {
            var taskStatus = await _context.TaskStatus
                .Where(t => t.TaskStatusID == id)
                .Select(t => new TAskStatusGetDTO
                {
                    TaskStatusID = t.TaskStatusID,
                    TaskStatusName = t.TaskStatusName,
                    TaskStatusCssClass = t.TaskStatusCssClass
                })
                .FirstOrDefaultAsync();

            if (taskStatus == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the TaskStatus with id : {id}"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"TaskStatus With {id} is Retrieved Successfully",
                Data = taskStatus
            });
        }

        // POST: api/TaskStatus
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> PostTaskStatus(TaskStatusPostDTO taskStatusDto)
        {
            var taskStatus = new TaskStatusModel
            {
                TaskStatusName = taskStatusDto.TaskStatusName,
                TaskStatusCssClass = taskStatusDto.TaskStatusCssClass
            };

            _context.TaskStatus.Add(taskStatus);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Object>
            {
                Success = true,
                Message = "TaskStatus Added Successfully",
                Data = taskStatus
            });
        }

        // PUT: api/TaskStatus/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateTaskStatus(
            int id,
            TaskStatusPostDTO taskStatusDto)
        {
            var oldTaskStatus = await _context.TaskStatus.FindAsync(id);

            if (oldTaskStatus == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the TaskStatus with id : {id}"
                });
            }

            oldTaskStatus.TaskStatusName = taskStatusDto.TaskStatusName;
            oldTaskStatus.TaskStatusCssClass = taskStatusDto.TaskStatusCssClass;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"TaskStatus with {id} is Submitted Successfully",
                Data = oldTaskStatus
            });
        }

        // DELETE: api/TaskStatus/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteTaskStatus(int id)
        {
            var taskStatus = await _context.TaskStatus.FindAsync(id);

            if (taskStatus == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the TaskStatus with id : {id}"
                });
            }

            _context.TaskStatus.Remove(taskStatus);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"TaskStatus With {id} Is Deleted  Successfully",
                Data = taskStatus
            });
        }
    }
}