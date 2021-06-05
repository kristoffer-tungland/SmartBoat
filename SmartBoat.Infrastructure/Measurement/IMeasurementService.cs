using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBoat.Infrastructure.Measurement
{
    public interface IMeasurementService
    {
        Task<Measurement> AddMeasurement(AddMeasurementArgs args);
        Task<Measurement> GetMeasurement(Guid id);
        Task<List<Measurement>> GetMeasurements();
    }
}