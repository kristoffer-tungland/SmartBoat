using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBoat.Infrastructure.Measurement
{
    public class MeasurementService : IMeasurementService
    {
        private readonly List<Measurement> _measurements;

        public MeasurementService(IMeasurementRepository repository)
        {
            _measurements = repository.Measurements;
        }

        public async Task<List<Measurement>> GetMeasurements()
        {
            await Task.Delay(0);
            return _measurements;
        }

        public async Task<Measurement> AddMeasurement(AddMeasurementArgs args)
        {
            await Task.Delay(0);

            var measurement = new Measurement
            {
                Id = Guid.NewGuid(),
                Value = args.Value
            };

            _measurements.Add(measurement);

            return measurement;
        }

        public async Task<Measurement> GetMeasurement(Guid id)
        {
            await Task.Delay(0);
            return _measurements.FirstOrDefault(x => x.Id == id);
        }
    }
}
