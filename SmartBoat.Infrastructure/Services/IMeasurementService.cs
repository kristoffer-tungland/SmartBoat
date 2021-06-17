using System.Collections.Generic;
using System.Threading.Tasks;
using SmartBoat.Shared.Models;

namespace SmartBoat.Infrastructure.Services
{
    public interface IMeasurementService
    {
        Task<List<Measurement>> GetMeasurements();
        Task<Measurement> AddMeasurement(AddMeasurementArgs args);
        Task<Measurement> GetMeasurement(string id);
    }
}