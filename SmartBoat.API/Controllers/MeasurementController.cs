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
    public class MeasurementController : ControllerBase
    {
        private readonly ILogger<MeasurementController> _logger;
        private readonly IMeasurementService measurementService;

        public MeasurementController(ILogger<MeasurementController> logger, IMeasurementService measurementService)
        {
            _logger = logger;
            this.measurementService = measurementService;
        }

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<Measurement>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMeasurements()
        {
            try
            {
                var measurements = await measurementService.GetMeasurements();

                return Ok(measurements);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Measurement),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMeasurement(AddMeasurementArgs args)
        {
            try
            {
                var measurement = await measurementService.AddMeasurement(args);

                return CreatedAtRoute(nameof(GetMeasurement), new { measurement.Id }, measurement);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}", Name = nameof(GetMeasurement))]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Measurement), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMeasurement(string id)
        {
            try
            {
                var measurement = await measurementService.GetMeasurement(id);

                if (measurement is null)
                    return NotFound($"Could not find measurement with id {id}");

                return Ok(measurement);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
