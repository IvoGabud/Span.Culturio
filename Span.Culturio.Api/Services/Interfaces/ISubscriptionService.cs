using Span.Culturio.Api.Models.DTOs;
using Span.Culturio.Api.Models.Entities;

namespace Span.Culturio.Api.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Subscription> CreateAsync(CreateSubscriptionDto dto);
        Task<List<Subscription>> GetSubscriptionsAsync(int? userId);
        Task<bool> TrackVisitAsync(TrackVisitDto dto);
        Task<bool> ActivateSubscriptionAsync(ActivateSubscriptionDto dto);
    }
}