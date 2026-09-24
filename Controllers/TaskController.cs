using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto.RoleDTO;
using SPMBACKENDSELF.Dto.TaskDTO;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Task
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty,Student")]
        public async Task<IActionResult> GetAllTask()
        {
            var allTask = await _context.Task
                .Select(t => new TaskGetDTO
                {
                    TaskID = t.TaskID,
                    ProjectAllocationID = t.ProjectAllocationID,
                    TaskTitle = t.TaskTitle,
                    TaskDescription = t.TaskDescription,
                    TaskStatusID = t.TaskStatusID,
                    TaskPriorityID = t.TaskPriorityID,
                    AssignedScore = t.AssignedScore,
                    EarnedScore = t.EarnedScore,

                    ProgressPercentage = (decimal)t.ProgressPercentage,

                    TaskAssignedDate = t.TaskAssignedDate,
                    TaskStartDate = t.TaskStartDate,
                    TaskDueDate = t.TaskDueDate,

                    TaskCompletedDate = t.TaskSCompleteDate,

                    NextFollowUpDate = t.NextFollowUpDate,
                    FacultyRemarks = t.FacultyRemarks,

                    StudentRemarks = t.StudeStudentRemarks
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<TaskGetDTO>>
            {
                Success = true,
                Message = "Task Retrieved Successfully",
                Data = allTask
            });
        }

        // GET: api/Task/1
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Faculty,Student")]

        public async Task<IActionResult> GetTaskByID(int id)
        {
            var task = await _context.Task
                .Where(t => t.TaskID == id)
                .Select(t => new TaskGetDTO
                {
                    TaskID = t.TaskID,
                    ProjectAllocationID = t.ProjectAllocationID,
                    TaskTitle = t.TaskTitle,
                    TaskDescription = t.TaskDescription,
                    TaskStatusID = t.TaskStatusID,
                    TaskPriorityID = t.TaskPriorityID,
                    AssignedScore = t.AssignedScore,
                    EarnedScore = t.EarnedScore,

                    ProgressPercentage = (decimal)t.ProgressPercentage,

                    TaskAssignedDate = t.TaskAssignedDate,
                    TaskStartDate = t.TaskStartDate,
                    TaskDueDate = t.TaskDueDate,

                    TaskCompletedDate = t.TaskSCompleteDate,

                    NextFollowUpDate = t.NextFollowUpDate,
                    FacultyRemarks = t.FacultyRemarks,

                    StudentRemarks = t.StudeStudentRemarks
                })
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Role with id : {id}"
                });
            }


            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = " ALL Task Retrieved Successfully",
                Data = task
            });
        }

        // POST: api/Task
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddTask(TaskPostDTO taskDto)
        {
            var task = new TaskModel
            {
                ProjectAllocationID = taskDto.ProjectAllocationID,
                TaskTitle = taskDto.TaskTitle,
                TaskDescription = taskDto.TaskDescription,
                TaskStatusID = taskDto.TaskStatusID,
                TaskPriorityID = taskDto.TaskPriorityID,
                AssignedScore = taskDto.AssignedScore,
                EarnedScore = taskDto.EarnedScore,

                ProgressPercentage = (double)taskDto.ProgressPercentage,

                TaskAssignedDate = taskDto.TaskAssignedDate,
                TaskStartDate = taskDto.TaskStartDate,
                TaskDueDate = taskDto.TaskDueDate,

                TaskSCompleteDate = taskDto.TaskCompletedDate,

                NextFollowUpDate = taskDto.NextFollowUpDate,
                FacultyRemarks = taskDto.FacultyRemarks,

                StudeStudentRemarks = taskDto.StudentRemarks
            };

            _context.Task.Add(task);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task Created Successfully",
                Data = task
            });
        }

        // PUT: api/Task/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> UpdateTask(
            int id,
            TaskPostDTO taskDto)
        {
            var oldTask = await _context.Task.FindAsync(id);

            if (oldTask == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Task with id : {id}"
                }); 
            }

            oldTask.ProjectAllocationID = taskDto.ProjectAllocationID;
            oldTask.TaskTitle = taskDto.TaskTitle;
            oldTask.TaskDescription = taskDto.TaskDescription;
            oldTask.TaskStatusID = taskDto.TaskStatusID;
            oldTask.TaskPriorityID = taskDto.TaskPriorityID;
            oldTask.AssignedScore = taskDto.AssignedScore;
            oldTask.EarnedScore = taskDto.EarnedScore;

            oldTask.ProgressPercentage = (double)taskDto.ProgressPercentage;

            oldTask.TaskAssignedDate = taskDto.TaskAssignedDate;
            oldTask.TaskStartDate = taskDto.TaskStartDate;
            oldTask.TaskDueDate = taskDto.TaskDueDate;

            oldTask.TaskSCompleteDate = taskDto.TaskCompletedDate;

            oldTask.NextFollowUpDate = taskDto.NextFollowUpDate;
            oldTask.FacultyRemarks = taskDto.FacultyRemarks;

            oldTask.StudeStudentRemarks = taskDto.StudentRemarks;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task Updated Successfully",
                Data = oldTask
            });
        }

        // DELETE: api/Task/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Task.FindAsync(id);

            if (task == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Task with id : {id}"
                });
            }

            _context.Task.Remove(task);

            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task Deleted Successfully",
                
            });
        }
    }
}