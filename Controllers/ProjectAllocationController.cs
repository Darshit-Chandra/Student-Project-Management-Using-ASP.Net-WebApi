using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto.ProjectAllocationDTO;
using SPMBACKENDSELF.Dto.RoleDTO;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProjectAllocationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectAllocationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectAllocation
        [Authorize(Roles = "Admin, Faculty")]
        [HttpGet]
        public async Task<IActionResult> GetAllProjectAllocation()
        {
            var projectAllocations = await _context.ProjectAllocation
                .Select(p => new ProjectAllocationGetDTO
                {
                    ProjectAllocationID = p.ProjectAllocationID,
                    ProjectID = p.ProjectID,
                    StudentID = p.StudentID,
                    FacultyID = p.FacultyID,
                    AssignedDate = p.AssignedDate,
                    ProjectStartDate = p.ProjectStartDate,
                    ProjectEndDate = p.ProjectEndDate,
                    TotalTasksGiven = p.TotalTasksGiven,
                    TotalCompletedTasks = p.TotalCompletedTasks,
                    ProgressPercentage = p.ProgressPercentage,
                    OverAllGrade = p.OverAllGrade
                })
                .ToListAsync();


            return Ok(new ApiResponse<List<ProjectAllocationGetDTO>>
            {
                Success = true,
                Message = "ProjectAllocation Retrieved Successfully",
                Data = projectAllocations
            });
        }

        // GET: api/ProjectAllocation/1
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Faculty")]

        public async Task<IActionResult> GetProjectAllocationById(int id)
        {
            var projectAllocation = await _context.ProjectAllocation
                .Where(p => p.ProjectAllocationID == id)
                .Select(p => new ProjectAllocationGetDTO
                {
                    ProjectAllocationID = p.ProjectAllocationID,
                    ProjectID = p.ProjectID,
                    StudentID = p.StudentID,
                    FacultyID = p.FacultyID,
                    AssignedDate = p.AssignedDate,
                    ProjectStartDate = p.ProjectStartDate,
                    ProjectEndDate = p.ProjectEndDate,
                    TotalTasksGiven = p.TotalTasksGiven,
                    TotalCompletedTasks = p.TotalCompletedTasks,
                    ProgressPercentage = p.ProgressPercentage,
                    OverAllGrade = p.OverAllGrade
                })
                .FirstOrDefaultAsync();

            if (projectAllocation == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Project with id : {id}"
                });
            }


            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "ProjectAllocation Retrieved Successfully",
                Data = projectAllocation
            });
        }

        // POST: api/ProjectAllocation
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddProjectAllocation(
            ProjectAllocationPostDTO projectAllocationDto)
        {
            var projectAllocation = new ProjectAllocationModel
            {
                ProjectID = projectAllocationDto.ProjectID,
                StudentID = projectAllocationDto.StudentID,
                FacultyID = projectAllocationDto.FacultyID,
                AssignedDate = projectAllocationDto.AssignedDate,
                ProjectStartDate = projectAllocationDto.ProjectStartDate,
                ProjectEndDate = projectAllocationDto.ProjectEndDate,
                TotalTasksGiven = projectAllocationDto.TotalTasksGiven,
                TotalCompletedTasks = projectAllocationDto.TotalCompletedTasks,
                ProgressPercentage = projectAllocationDto.ProgressPercentage,
                OverAllGrade = projectAllocationDto.OverAllGrade
            };

            _context.ProjectAllocation.Add(projectAllocation);

            await _context.SaveChangesAsync();


            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "ProjectAllocation Sent Successfully",
                Data = projectAllocation
            });
        }

        // PUT: api/ProjectAllocation/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateProjectAllocation(
            int id,
            ProjectAllocationPostDTO projectAllocationDto)
        {
            var oldProjectAllocation =
                await _context.ProjectAllocation.FindAsync(id);

            if (oldProjectAllocation == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Project with id : {id}"
                });
            }

            oldProjectAllocation.ProjectID = projectAllocationDto.ProjectID;
            oldProjectAllocation.StudentID = projectAllocationDto.StudentID;
            oldProjectAllocation.FacultyID = projectAllocationDto.FacultyID;
            oldProjectAllocation.AssignedDate = projectAllocationDto.AssignedDate;
            oldProjectAllocation.ProjectStartDate = projectAllocationDto.ProjectStartDate;
            oldProjectAllocation.ProjectEndDate = projectAllocationDto.ProjectEndDate;
            oldProjectAllocation.TotalTasksGiven = projectAllocationDto.TotalTasksGiven;
            oldProjectAllocation.TotalCompletedTasks = projectAllocationDto.TotalCompletedTasks;
            oldProjectAllocation.ProgressPercentage = projectAllocationDto.ProgressPercentage;
            oldProjectAllocation.OverAllGrade = projectAllocationDto.OverAllGrade;

            await _context.SaveChangesAsync();


            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "ProjectAllocation Updated Successfully",
                Data = oldProjectAllocation
            });
        }

        // DELETE: api/ProjectAllocation/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteProjectAllocation(int id)
        {
            var projectAllocation =
                await _context.ProjectAllocation.FindAsync(id);

            if (projectAllocation == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Role with id : {id}"
                });
            }

            _context.ProjectAllocation.Remove(projectAllocation);

            await _context.SaveChangesAsync();


            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "ProjectAllocation Deleted Successfully",
            });
        }
    }
}