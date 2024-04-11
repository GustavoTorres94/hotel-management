using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("hotel")]
    public class HotelController : Controller
    {
        private readonly IHotelRepository _repository;

        public HotelController(IHotelRepository repository)
        {
            _repository = repository;
        }
        
        // 4. Desenvolva o endpoint GET /hotel
        [HttpGet]
        public IActionResult GetHotels(){
            try
            {
                return Ok(_repository.GetHotels());
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // 5. Desenvolva o endpoint POST /hotel
        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize (Policy = "Admin")]
        public IActionResult PostHotel([FromBody] Hotel hotel){
            try
            {
                return Created("AddHotel", _repository.AddHotel(hotel));
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        

    }
}