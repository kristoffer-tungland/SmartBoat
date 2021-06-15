using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBoat.Infrastructure.Services;
using SmartBoat.Shared.Models;

namespace SmartBoat.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoatController : ControllerBase
    {
        private readonly ILogger<BoatController> _logger;
        private readonly IBoatService boatService;

        public BoatController(ILogger<BoatController> logger, IBoatService boatService)
        {
            _logger = logger;
            this.boatService = boatService;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<Boat>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBoats()
        {
            try
            {
                var boats = await boatService.GetBoats();

                return Ok(boats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Boat),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBoat(AddBoatArgs args)
        {
            try
            {
                var boat = await boatService.AddBoat(args);

                return CreatedAtRoute(nameof(GetBoat), new { boat.Id }, boat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}", Name = nameof(GetBoat))]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Boat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBoat(string id)
        {
            try
            {
                var boat = await boatService.GetBoat(id);

                if (boat is null)
                    return NotFound($"Could not find boat with id {id}");

                return Ok(boat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Boat), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBoat(Boat boat)
        {
            try
            {
                boat = await boatService.UpdateBoat(boat);

                return Ok(boat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBoat(string id)
        {
            try
            {
                await boatService.DeleteBoat(id);

                return Ok("Boat deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
