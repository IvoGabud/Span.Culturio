using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.Models.DTOs;
using Span.Culturio.Api.Models.Entities;
using Span.Culturio.Api.Services.Interfaces;

namespace Span.Culturio.Api.Services
{
    public class CultureObjectService : ICultureObjectService
    {
        private readonly CulturioDbContext _context;

        public CultureObjectService(CulturioDbContext context)
        {
            _context = context;
        }

        public async Task<CultureObject> CreateAsync(CreateCultureObjectDto dto)
        {
            var cultureObject = new CultureObject
            {
                Name = dto.Name,
                ContactEmail = dto.ContactEmail,
                Address = dto.Address,
                ZipCode = dto.ZipCode,
                City = dto.City,
                AdminUserId = dto.AdminUserId
            };

            _context.CultureObjects.Add(cultureObject);
            await _context.SaveChangesAsync();

            return cultureObject;
        }

        public async Task<List<CultureObject>> GetAllAsync()
        {
            return await _context.CultureObjects.ToListAsync();
        }

        public async Task<CultureObject?> GetByIdAsync(int id)
        {
            return await _context.CultureObjects.FindAsync(id);
        }
    }
}