using CC.API.Service;
using Microsoft.AspNetCore.Mvc;

namespace CC.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet(Name ="GetUsers")]
        public async Task<IActionResult> Get()
        {
            var user = await _usersService.GetAllUsers();

            if (user.Any()) { 
                return Ok(user);
            }

            return NotFound();
        }
    }
}
