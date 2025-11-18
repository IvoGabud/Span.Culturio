using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Span.Culturio.Api.Models.DTOs;
using Span.Culturio.Api.Services.Interfaces;

namespace Span.Culturio.Api.Controllers
{
    [Route("subscriptions")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionDto dto)
        {
            try
            {
                var subscription = await _subscriptionService.CreateAsync(dto);
                return Ok(new { message = "Subscription created", id = subscription.Id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSubscriptions([FromQuery] int? userId)
        {
            var subscriptions = await _subscriptionService.GetSubscriptionsAsync(userId);

            return Ok(subscriptions.Select(s => new
            {
                s.Id,
                s.UserId,
                s.PackageId,
                s.Name,
                s.ActiveFrom,
                s.ActiveTo,
                s.State,
                s.RecordedVisits
            }));
        }

        [HttpPost("track-visit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TrackVisit([FromBody] TrackVisitDto dto)
        {
            var result = await _subscriptionService.TrackVisitAsync(dto);

            if (!result)
                return BadRequest(new { message = "Unable to track visit" });

            return Ok(new { message = "Visit tracked successfully" });
        }

        [HttpPost("activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivateSubscription([FromBody] ActivateSubscriptionDto dto)
        {
            var result = await _subscriptionService.ActivateSubscriptionAsync(dto);

            if (!result)
                return BadRequest(new { message = "Unable to activate subscription" });

            return Ok(new { message = "Subscription activated successfully" });
        }
    }
}
