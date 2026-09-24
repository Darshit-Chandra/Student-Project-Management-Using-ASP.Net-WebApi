using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPMBACKENDSELF.ApiCommonResponse;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto;
using SPMBACKENDSELF.Dto.TaskStatusDTo;
using SPMBACKENDSELF.Dto.UserDTO;
using SPMBACKENDSELF.Models;

namespace SPMBACKENDSELF.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.User
                .Select(u => new UserGetDTO
                {
                    UserID = u.UserID,
                    UserTypeID = u.UserTypeID,
                    FullName = u.FullName,
                    UserCode = u.UserCode,
                    Email = u.Email,
                    MobileNumber = u.MobileNumber,
                    ProfilePicturePath = u.ProfilePicturePath,
                    IsActive = u.IsActive,
                    IsDeleted = u.IsDeleted
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User Retrieved Successfully",
                Data = users
            });
        }

        // GET: api/User/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.User
                .Where(u => u.UserID == id)
                .Select(u => new UserGetDTO
                {
                    UserID = u.UserID,
                    UserTypeID = u.UserTypeID,
                    FullName = u.FullName,
                    UserCode = u.UserCode,
                    Email = u.Email,
                    MobileNumber = u.MobileNumber,
                    ProfilePicturePath = u.ProfilePicturePath,
                    IsActive = u.IsActive,
                    IsDeleted = u.IsDeleted
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the User with id : {id}"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"User With {id} Is Retrieved Successfully",
                Data = user
            });
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> AddUser(UserPostDTO userDto)
        {
            var user = new UserModel
            {
                UserTypeID = userDto.UserTypeID,
                FullName = userDto.FullName,
                UserCode = userDto.UserCode,
                Email = userDto.Email,
                Password = userDto.Password,
                MobileNumber = userDto.MobileNumber,
                ProfilePicturePath = userDto.ProfilePicturePath,
                IsActive = userDto.IsActive,
               
            };

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User Added Successfully",
                Data = user
            });
        }

        // PUT: api/User/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            int id,
            UserPostDTO userDto)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the User with id : {id}"
                });
            }

            user.UserTypeID = userDto.UserTypeID;
            user.FullName = userDto.FullName;
            user.UserCode = userDto.UserCode;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.MobileNumber = userDto.MobileNumber;
            user.ProfilePicturePath = userDto.ProfilePicturePath;
            user.IsActive = userDto.IsActive;
            

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"User With {id} Is Updated Successfully",
                Data = user
            });
        }

        // DELETE: api/User/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Cannot find the User with id : {id}"
                });
            }

            _context.User.Remove(user);

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = $"User With {id} Is Deleted  Successfully",
                Data = user
            });
        }
    }
}