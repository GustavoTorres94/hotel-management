using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
  
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert){
         
            var room = _repository.GetRoomById(bookingInsert.RoomId) ?? throw new Exception("Room not found");
            if (room.Capacity < bookingInsert.GuestQuant)
            {
                return BadRequest(new { message = "Guest quantity over room capacity" });
            }
            var adding = _repository.Add(bookingInsert, User.FindFirst(ClaimTypes.Email)!.Value);
            return Created("BookingPost", adding);
        
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid){
           try
           {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var booking = _repository.GetBooking(Bookingid, userEmail!);
                               
                return Ok(booking);
           }
           catch (Exception e)
           {
                return BadRequest(new { message = "Error getting booking: " + e.Message });
           }
        }
    }
}