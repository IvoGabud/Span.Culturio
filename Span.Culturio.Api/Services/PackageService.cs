using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.Models.Entities;
using Span.Culturio.Api.Services.Interfaces;

namespace Span.Culturio.Api.Services
{
    public class PackageService : IPackageService
    {
        private readonly CulturioDbContext _context;

        public PackageService(CulturioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Package>> GetAllAsync()
        {
            return await _context.Packages
                .Include(p => p.PackageCultureObjects)
                .ToListAsync();
        }
    }
}