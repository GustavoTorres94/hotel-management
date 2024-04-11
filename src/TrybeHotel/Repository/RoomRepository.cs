using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var findRoom = from room in _context.Rooms
                           where room.HotelId == HotelId
                           select new RoomDto
                           {
                               RoomId = room.RoomId,
                               Name = room.Name,
                               Capacity = room.Capacity,
                               Image = room.Image,
                               Hotel = new HotelDto
                               {
                                   HotelId = room.Hotel!.HotelId,
                                   Name = room.Hotel.Name,
                                   Address = room.Hotel.Address,
                                   CityId = room.Hotel.CityId,
                                   CityName = room.Hotel.City!.Name,
                                   State = room.Hotel.City.State
                               }
                           };
            return findRoom;
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room) {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            var result = from e in _context.Rooms
                         where e.RoomId == room.RoomId
                         select new RoomDto
                         {
                             RoomId = e.RoomId,
                             Name = e.Name,
                             Capacity = e.Capacity,
                             Image = e.Image,
                             Hotel = new HotelDto
                             {
                                 HotelId = e.Hotel!.HotelId,
                                 Name = e.Hotel.Name,
                                 Address = e.Hotel.Address,
                                 CityId = e.Hotel.CityId,
                                 CityName = e.Hotel.City!.Name,
                                 State = e.Hotel.City.State
                             }
                         };
            return result.ToList()[0];
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        public void DeleteRoom(int RoomId) {
            var room = _context.Rooms.Find(RoomId);
            _context.Rooms.Remove(room!);
            _context.SaveChanges();
        }
    }
}