using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto.UserRoleDTO;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserRoleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserRole
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userRoles = await _context.UserRoles
                .Select(ur => new UserRoleGetDTO
                {
                    UserID = ur.UserID,
                    RoleID = ur.RoleID,
                    RolePermissionID = ur.RolePermissionID
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"User With Retrieved Successfully",
                Data = userRoles
            });
        }
            // GET: api/UserRole/1
            [HttpGet("{id}")]
             public async Task<IActionResult> GetById(int id)
           {
            var userRole = await _context.UserRoles
                .Where(ur => ur.RolePermissionID == id)
                .Select(ur => new UserRoleGetDTO
                {
                    
                    UserID = ur.UserID,
                    RoleID = ur.RoleID,
                    RolePermissionID = ur.RolePermissionID
                })
                .FirstOrDefaultAsync();

            if (userRole == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the UserRole with id : {id}"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"UserRole With {id} Is Retrieved Successfully",
                Data = userRole
            });
        }

        // POST: api/UserRole
        [HttpPost]
        public async Task<IActionResult> AddUserRole(UserRolePostDTO userRoleDto)
        {
            var userRole = new UserRoleModel
            {
                UserID = userRoleDto.UserID,
                RoleID = userRoleDto.RoleID,
            };

            _context.UserRoles.Add(userRole);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"User Is Added Successfully",
                Data = userRole
            });
        }

        // PUT: api/UserRole/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserRole(
            int id,
            UserRolePostDTO userRoleDto)
        {
            var userRole = await _context.UserRoles.FindAsync(id);

            if (userRole == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the User with id : {id}"
                });
            }

            userRole.UserID = userRoleDto.UserID;
            userRole.RoleID = userRoleDto.RoleID;
          

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"UserRole With {id} Is Updated Successfully",
                Data = userRole
            });
        }

        // DELETE: api/UserRole/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);

            if (userRole == null)
            {
                return NotFound();
            }

            _context.UserRoles.Remove(userRole);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"User With {id} Is Deleted Successfully",
                Data = userRole
            });
        }
    }
}