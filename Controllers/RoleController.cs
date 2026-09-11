using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto.RoleDTO;   
using SPMBACKENDSELF.Models;
using System.Data;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<RolePostDTO> _roleValidator;
        public RoleController(AppDbContext context, IValidator<RolePostDTO> roleValidator) {
            _context = context;
            _roleValidator = roleValidator;
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Role
         .Select(r => new RoleGetDTO
         {
             RoleID = r.RoleID,
             RoleName = r.RoleName,
             Description = r.Description
         })
         .ToListAsync();

            return Ok(new ApiResponse<List<RoleGetDTO>>
            {
                Success = true,
                Message = "Role Retrieved Successfully",
                Data = roles
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRolesByID(int id)
        {
            var role = await _context.Role
          .Where(r => r.RoleID == id)
          .Select(r => new RoleGetDTO
          {
              RoleID = r.RoleID,
              RoleName = r.RoleName,
              Description = r.Description
          })
          .FirstOrDefaultAsync();

            if (role == null)
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
                Message = "Role Retrieved Successfully",
                Data = role
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RolePostDTO roleDTO)
        {
            var role = new RoleModel
            {
                RoleName = roleDTO.RoleName,
                Description = roleDTO.Description
            };

            _context.Role.Add(role);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Role Added Successfully",
                Data = role
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RolePostDTO roleDto)
        {
            var oldRole = await _context.Role.FindAsync(id);

            if (oldRole == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Role with id : {id}"
                });
            }

            oldRole.RoleName = roleDto.RoleName;
            oldRole.Description = roleDto.Description;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Role Updated Successfully",
                Data = oldRole 
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByID(int id)
        {
            var role = await _context.Role.FindAsync(id);

            if (role == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the Role with id : {id}"
                });
            }

            _context.Role.Remove(role);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Role Deleted Successfully",
                
            });
        }
    }
}