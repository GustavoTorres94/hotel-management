using TrybeHotel.Models;
using TrybeHotel.Dto;
using System;
using Microsoft.AspNetCore.Authorization;
using TrybeHotel.Services;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }
        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
            if (user == null || user.Password != login.Password)
            {
                throw new Exception("Incorrect e-mail or password");
            }               
            var userDto = from u in _context.Users
                          where u.Email == login.Email
                          select new UserDto
                          {
                              UserId = u.UserId,
                              Name = u.Name,
                              Email = u.Email,
                              UserType = u.UserType
                          };
            return userDto.First();
        }
        public UserDto Add(UserDtoInsert user)
        { 
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return new UserDto
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return null;
            return new UserDto
            {
                UserId = user.UserId!,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };
        }

        public IEnumerable<UserDto> GetUsers()
        {
           return _context.Users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            });
        }

    }
}