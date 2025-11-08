using Span.Culturio.Api.Models.DTOs;
using Span.Culturio.Api.Models.Entities;

namespace Span.Culturio.Api.Services.Interfaces
{
    public interface ICultureObjectService
    {
        Task<CultureObject> CreateAsync(CreateCultureObjectDto dto);
        Task<List<CultureObject>> GetAllAsync();
        Task<CultureObject?> GetByIdAsync(int id);
    }
}