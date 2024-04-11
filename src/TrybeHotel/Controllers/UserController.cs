using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers(){
            var users = _repository.GetUsers();
            return Ok(users.ToList());
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            try
            {
                var userExists = _repository.GetUserByEmail(user.Email!);
          
                if (userExists != null)
                {
                    var error = new { message = "User email already exists"};
                    return Conflict(error);
                }
                var newUser = _repository.Add(user);
                return Created("PostUser", newUser);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}