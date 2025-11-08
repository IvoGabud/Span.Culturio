using Span.Culturio.Api.Models.Entities;

  namespace Span.Culturio.Api.Services.Interfaces
{
    public interface IPackageService
    {
        Task<List<Package>> GetAllAsync();
    }
}