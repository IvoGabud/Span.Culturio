using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.Models.Entities;
using Span.Culturio.Api.Services.Interfaces;

namespace Span.Culturio.Api.Services
{
    public class UserService : IUserService
    {
        private readonly CulturioDbContext _context;

        public UserService(CulturioDbContext context)
        {
            _context = context;
        }

        public async Task<(List<User> users, int totalCount)> GetUsersAsync(int pageIndex, int pageSize)
        {
            var totalCount = await _context.Users.CountAsync();

            var users = await _context.Users
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(u => new User
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username
                })
                .ToListAsync();

            return (users, totalCount);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return null;

            return new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username
            };
        }
    }
}