using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto.ProjectAllocationDTO;
using SPMBACKENDSELF.Dto.ProjectMasterDTO;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectMasterController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<ProjectMasterPostDTO> _projectValidator;

        public ProjectMasterController(AppDbContext context, IValidator<ProjectMasterPostDTO> projectValidator)
        {
            _context = context;
            _projectValidator = projectValidator;
        }

        // GET: api/ProjectMaster
        [HttpGet]
        public async Task<IActionResult> GetAllProject()
        {
            var projects = await _context.projectMaster
                .Select(p => new ProjectMasterGetDTO
                {
                    ProjectID = p.ProjectID,
                    ProjectTitle = p.ProjectTitle,
                    Description = p.Description
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<ProjectMasterGetDTO>>
            {
                Success = true,
                Message = "Projects Retrieved Successfully",
                Data = projects
            });
        }

        // GET: api/ProjectMaster/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectByID(int id)
        {
            var project = await _context.projectMaster
                .Where(p => p.ProjectID == id)
                .Select(p => new ProjectMasterGetDTO
                {
                    ProjectID = p.ProjectID,
                    ProjectTitle = p.ProjectTitle,
                    Description = p.Description
                })
                .FirstOrDefaultAsync();

            if (project == null)
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
                Message = $"Project with {id}  is Retrieved Successfully",
                Data = project
            });
        }

        // POST: api/ProjectMaster
        [HttpPost]
        public async Task<IActionResult> PostProject(
            ProjectMasterPostDTO projectDto)
        {
            var project = new ProjectMasterModel
            {
                ProjectTitle = projectDto.ProjectTitle,
                Description = projectDto.Description
            };

            _context.projectMaster.Add(project);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"Project Submitted Successfully",
                Data = project
            });
        }

        // PUT: api/ProjectMaster/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(
            int id,
            ProjectMasterPostDTO projectDto)
        {
            var project = await _context.projectMaster.FindAsync(id);

            if (project == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Project is Not submitted"
                });
            }

            project.ProjectTitle = projectDto.ProjectTitle;
            project.Description = projectDto.Description;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"Project with {id} Is Updated Successfully",
                Data = project
            });
        }

        // DELETE: api/ProjectMaster/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.projectMaster.FindAsync(id);

            if (project == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Project with id : {id}"
                });
            }

            _context.projectMaster.Remove(project);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<List<ProjectMasterGetDTO>>
            {
                Success = true,
                Message = "Project Deleted Successfully",
               
            });
        }
    }
}