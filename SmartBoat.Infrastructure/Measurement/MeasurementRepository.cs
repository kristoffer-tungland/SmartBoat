using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBoat.Infrastructure.Measurement
{
    internal class MeasurementRepository : IMeasurementRepository
    {
        public MeasurementRepository()
        {
            Measurements = new List<Measurement>();
            Measurements.Add(new Measurement { Id = Guid.NewGuid(), Value = 39.0 });
            Measurements.Add(new Measurement { Id = Guid.NewGuid(), Value = 64.0 });
            Measurements.Add(new Measurement { Id = Guid.NewGuid(), Value = 36.0 });
        }

        public List<Measurement> Measurements { get; private set; }
    }
}