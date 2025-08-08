using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskMaster.Services;

namespace TaskMaster.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context) 
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser() 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return Unauthorized(); }

            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            if (user == null) { return NotFound(); }
            return Ok(new 
            { user.Id,
            user.Username,
            user.Email
            });
        }

    }
}
