using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SmartBoat.Infrastructure.Services;
using SmartBoat.Shared.Models;

namespace SmartBoat.API.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class MeasurementController : ControllerBase
    {
        private readonly ILogger<MeasurementController> _logger;
        private readonly IMeasurementService measurementService;
        private HubConnection hubConnection;

        public MeasurementController(ILogger<MeasurementController> logger, IMeasurementService measurementService)
        {
            _logger = logger;
            this.measurementService = measurementService;
            hubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:5001/sensorData"))
            .Build();
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

                await OnSensorUpdate(measurement);

                return CreatedAtRoute(nameof(GetMeasurement), new { measurement.Id }, measurement);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        private async Task OnSensorUpdate(Measurement measurement)
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
                await hubConnection.StartAsync();

            await hubConnection.SendAsync("SenorUpdate", measurement.Id, measurement.Value);
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
