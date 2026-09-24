using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto;
using SPMBACKENDSELF.Dto.UserTypeDTO;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserTypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<UserTypePostDTO> _userTypeValidator;

        public UserTypeController(AppDbContext context, IValidator<UserTypePostDTO> userTypeValidator)
        {
            _context = context;
            _userTypeValidator = userTypeValidator;
        }

        // GET: api/UserType
        [HttpGet]
        public async Task<IActionResult> GetAllUserType()
        {
            var userTypes = await _context.UserTypes
                .Select(u => new UserTypeGetDTO
                {
                    UserTypeID = u.UserTypeID,
                    UserTypeName = u.UserTypeName,
                    Description = u.Description
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"UserType Retrieved Successfully",
                Data = userTypes
            });
        }

        // GET: api/UserType/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserTypeByID(int id)
        {
            var userType = await _context.UserTypes
                .Where(u => u.UserTypeID == id)
                .Select(u => new UserTypeGetDTO
                {
                    UserTypeID = u.UserTypeID,
                    UserTypeName = u.UserTypeName,
                    Description = u.Description
                })
                .FirstOrDefaultAsync();

            if (userType == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the UserType with id : {id}"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"UserType With {id} Is Retrieved Successfully",
                Data = userType
            });
        }

        // POST: api/UserType
        [HttpPost]
        public async Task<IActionResult> AddUserType(UserTypePostDTO userTypeDto)
        {
            var userType = new UserTypeModel
            {
                UserTypeName = userTypeDto.UserTypeName,
                Description = userTypeDto.Description
            };

            _context.UserTypes.Add(userType);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"UserType Is Added Successfully",
                Data = userType
            });
        }

        // PUT: api/UserType/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserType(
            int id,
            UserTypePostDTO userTypeDto)
        {
            var oldUserType = await _context.UserTypes.FindAsync(id);

            if (oldUserType == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the UserRole with id : {id}"
                });
            }

            oldUserType.UserTypeName = userTypeDto.UserTypeName;
            oldUserType.Description = userTypeDto.Description;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"UserType With {id} Is Updated Successfully",
                Data = oldUserType
            });
        }

        // DELETE: api/UserType/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            var userType = await _context.UserTypes.FindAsync(id);

            if (userType == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the UserRole with id : {id}"
                });
            }

            _context.UserTypes.Remove(userType);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"UserRole With {id} Is Deleted Successfully",
                Data = userType
            });
        }
    }
}