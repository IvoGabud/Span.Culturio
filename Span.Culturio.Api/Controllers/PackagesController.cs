using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Span.Culturio.Api.Services.Interfaces;

namespace Span.Culturio.Api.Controllers
{
    [Route("packages")]
    [ApiController]
    [Authorize]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var packages = await _packageService.GetAllAsync();

            return Ok(packages.Select(p => new
            {
                p.Id,
                p.Name,
                cultureObjects = p.PackageCultureObjects.Select(pco => new
                {
                    id = pco.CultureObjectId,
                    availableVisits = pco.AvailableVisits
                }),
                p.ValidDays
            }));
        }
    }
}
