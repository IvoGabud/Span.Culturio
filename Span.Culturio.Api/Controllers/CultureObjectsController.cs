using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Span.Culturio.Api.Models.DTOs;
using Span.Culturio.Api.Services.Interfaces;

namespace Span.Culturio.Api.Controllers
{
    [Route("culture-objects")]
    [ApiController]
    [Authorize]
    public class CultureObjectsController : ControllerBase
    {
        private readonly ICultureObjectService _cultureObjectService;

        public CultureObjectsController(ICultureObjectService cultureObjectService)
        {
            _cultureObjectService = cultureObjectService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCultureObjectDto dto)
        {
            try
            {
                var cultureObject = await _cultureObjectService.CreateAsync(dto);
                return Ok(new { message = "Culture object created", id = cultureObject.Id });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var cultureObjects = await _cultureObjectService.GetAllAsync();

            return Ok(cultureObjects.Select(co => new
            {
                co.Id,
                co.Name,
                co.ContactEmail,
                co.ZipCode,
                co.Address,
                co.City,
                co.AdminUserId
            }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var cultureObject = await _cultureObjectService.GetByIdAsync(id);

            if (cultureObject == null)
                return NotFound();

            return Ok(new
            {
                cultureObject.Id,
                cultureObject.Name,
                cultureObject.ContactEmail,
                cultureObject.ZipCode,
                cultureObject.Address,
                cultureObject.City,
                cultureObject.AdminUserId
            });
        }
    }
}
