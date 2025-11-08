using Span.Culturio.Api.Models.Entities;

  namespace Span.Culturio.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<(List<User> users, int totalCount)> GetUsersAsync(int pageIndex, int pageSize);
        Task<User?> GetUserByIdAsync(int id);
    }
}