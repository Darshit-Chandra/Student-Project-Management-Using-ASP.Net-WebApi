using SPMBACKENDSELF.Models;
using SPMBACKENDSELF.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPMBACKENDSELF.Data;
using SPMBACKENDSELF.Dto.UserLoginDTO;
using SPMBACKENDSELF.Services;
using Microsoft.EntityFrameworkCore;

namespace SPMBACKENDSELF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly TokenServices _tokenService;
        private readonly AppDbContext _context;

        public AuthController (AppDbContext context, TokenServices tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            try
            {
                var user = await _context.User
                                               .SingleOrDefaultAsync(u =>
                                               u.Email == dto.Email &&
                                               u.Password == dto.Password);
                if (user == null)
                {
                    return Unauthorized("Invalid Email or password");
                }
                var token = _tokenService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }

        //Not Needed Manually to [Authorize] Keyword
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("This is protected data!");
        }
        [AllowAnonymous] // Override controller-level [Authorize] And Make Public Method
        [HttpGet]
        public IActionResult GetAllStudentsByCategory()
        {
            return Ok("This is Public data!");
        }
    }
}