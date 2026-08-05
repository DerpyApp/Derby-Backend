using Microsoft.AspNetCore.Mvc;
using PadelBooking.API.DTOs;
using PadelBooking.API.Services;

namespace PadelBooking.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /api/users
        // GET: /api/users?searchTerm=Amal
        // GET: /api/users?role=Player
        // GET: /api/users?searchTerm=Amal&role=Player
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterDto filter)
        {
            var users = await _userService.GetAllUsersAsync(filter);
            return Ok(users);
        }

        // GET: /api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "المستخدم غير موجود!" });

            return Ok(user);
        }
    }
}