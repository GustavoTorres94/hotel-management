using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _repository;
        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        [HttpGet("{HotelId}")]
        public IActionResult GetRoom(int HotelId){
            return Ok(_repository.GetRooms(HotelId));
        }

        // 7. Desenvolva o endpoint POST /room
        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize (Policy = "Admin")]
        public IActionResult PostRoom([FromBody] Room room){
            try
            {
                if (room == null)
                {
                    return BadRequest("Room is null.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }
                return Created("PostRoom", _repository.AddRoom(room));
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        [HttpDelete("{RoomId}")]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize (Policy = "Admin")]
        public IActionResult Delete(int RoomId)
        {
            try
            {                
                _repository.DeleteRoom(RoomId);
                return NoContent();
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}