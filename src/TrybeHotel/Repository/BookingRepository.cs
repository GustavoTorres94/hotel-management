using TrybeHotel.Models;
using TrybeHotel.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            try
            {
                var bookingEntity = new Booking
                {
                    CheckIn = booking.CheckIn,
                    CheckOut = booking.CheckOut,
                    GuestQuant = booking.GuestQuant,
                    RoomId = booking.RoomId,
                    UserId = _context.Users.FirstOrDefault(u => u.Email == email)!.UserId
                };

                _context.Bookings.Add(bookingEntity);
                _context.SaveChanges();

                var result = from b in _context.Bookings
                             join r in _context.Rooms on b.RoomId equals r.RoomId
                             join h in _context.Hotels on r.HotelId equals h.HotelId
                             where b.BookingId == bookingEntity.BookingId
                             select new BookingResponse
                             {
                                 BookingId = b.BookingId,
                                 CheckIn = b.CheckIn,
                                 CheckOut = b.CheckOut,
                                 GuestQuant = b.GuestQuant,
                                 Room = new RoomDto
                                 {
                                     RoomId = r.RoomId,
                                     Name = r.Name,
                                     Capacity = r.Capacity,
                                     Image = r.Image,
                                     Hotel = new HotelDto
                                     {
                                         HotelId = h.HotelId,
                                         Name = h.Name,
                                         Address = h.Address,
                                         CityId = h.CityId,
                                         CityName = h.City!.Name,
                                         State = h.City.State
                                     }
                                 }                    
                             };
                    return result.FirstOrDefault()!;
            }
            catch (Exception e)
            {
                throw new Exception("Error adding booking: " + e);
            }
        }
        

        public BookingResponse GetBooking(int bookingId, string email)
        {
            try
            {
                var result = from b in _context.Bookings
                             join r in _context.Rooms on b.RoomId equals r.RoomId
                             join h in _context.Hotels on r.HotelId equals h.HotelId
                             where b.BookingId == bookingId && _context.Users.FirstOrDefault(u => u.Email == email)!.UserId == b.UserId
                             select new BookingResponse
                             {
                                 BookingId = b.BookingId,
                                 CheckIn = b.CheckIn,
                                 CheckOut = b.CheckOut,
                                 GuestQuant = b.GuestQuant,
                                 Room = new RoomDto
                                 {
                                     RoomId = r.RoomId,
                                     Name = r.Name,
                                     Capacity = r.Capacity,
                                     Image = r.Image,
                                     Hotel = new HotelDto
                                     {
                                         HotelId = h.HotelId,
                                         Name = h.Name,
                                         Address = h.Address,
                                         CityId = h.CityId,
                                         CityName = h.City!.Name,
                                         State = h.City.State
                                     }
                                 }
                             };
                return result.FirstOrDefault()!;                
            }
            catch (Exception e)
            {
                throw new Exception("Error getting booking: " + e);
            }
        }

        public Room GetRoomById(int RoomId)
        {
            try
            {
                return _context.Rooms.Find(RoomId)!;
            }
            catch (Exception e)
            {
                throw new Exception("Error getting room: " + e);
            }
        }

    }

}