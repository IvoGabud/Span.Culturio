using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.Models.DTOs;
using Span.Culturio.Api.Models.Entities;
using Span.Culturio.Api.Services.Interfaces;

namespace Span.Culturio.Api.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly CulturioDbContext _context;

        public SubscriptionService(CulturioDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription> CreateAsync(CreateSubscriptionDto dto)
        {
            var subscription = new Subscription
            {
                UserId = dto.UserId,
                PackageId = dto.PackageId,
                Name = dto.Name,
                State = "expired",
                RecordedVisits = 0,
                ActiveFrom = null,
                ActiveTo = null
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }

        public async Task<List<Subscription>> GetSubscriptionsAsync(int? userId)
        {
            var query = _context.Subscriptions
                .Include(s => s.Package)
                .AsQueryable();

            if (userId.HasValue)
                query = query.Where(s => s.UserId == userId.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> TrackVisitAsync(TrackVisitDto dto)
        {
            var subscription = await _context.Subscriptions
                .Include(s => s.Package)
                    .ThenInclude(p => p.PackageCultureObjects)
                .FirstOrDefaultAsync(s => s.Id == dto.SubscriptionId);

            if (subscription == null || subscription.State != "active")
                return false;

            if (subscription.ActiveTo.HasValue && subscription.ActiveTo.Value < DateTime.UtcNow)
            {
                subscription.State = "expired";
                await _context.SaveChangesAsync();
                return false;
            }

            var packageCultureObject = subscription.Package.PackageCultureObjects
                .FirstOrDefault(pco => pco.CultureObjectId == dto.CultureObjectId);

            if (packageCultureObject == null)
                return false;

            var visitsToThisCultureObject = await _context.Visits
                .CountAsync(v => v.SubscriptionId == dto.SubscriptionId
                    && v.CultureObjectId == dto.CultureObjectId);

            if (visitsToThisCultureObject >= packageCultureObject.AvailableVisits)
                return false;

            var visit = new Visit
            {
                SubscriptionId = dto.SubscriptionId,
                CultureObjectId = dto.CultureObjectId,
                VisitDate = DateTime.UtcNow
            };

            _context.Visits.Add(visit);
            subscription.RecordedVisits++;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateSubscriptionAsync(ActivateSubscriptionDto dto)
        {
            var subscription = await _context.Subscriptions
                .Include(s => s.Package)
                .FirstOrDefaultAsync(s => s.Id == dto.SubscriptionId);

            if (subscription == null)
                return false;

            subscription.State = "active";
            subscription.ActiveFrom = DateTime.UtcNow;
            subscription.ActiveTo = DateTime.UtcNow.AddDays(subscription.Package.ValidDays);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}