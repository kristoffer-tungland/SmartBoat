using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SmartBoat.Infrastructure.Models;
using SmartBoat.Infrastructure.Repository;
using SmartBoat.Shared.Models;

namespace SmartBoat.Infrastructure.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMongoRepository<MeasurementStorage> _measurementRepository;
        private readonly IMapper _mapper;

        public MeasurementService(IMongoRepository<MeasurementStorage> measurementRepository, IMapper mapper)
        {
            _measurementRepository = measurementRepository;
            _mapper = mapper;
        }

        public async Task<List<Measurement>> GetMeasurements()
        {
            await Task.Delay(0);
            var items = _measurementRepository.FilterBy(x => true);

            return _mapper.Map<List<Measurement>>(items);
        }

        public async Task<Measurement> AddMeasurement(AddMeasurementArgs args)
        {
            var measurement = _mapper.Map<MeasurementStorage>(args);

            await _measurementRepository.InsertOneAsync(measurement);

            return _mapper.Map<Measurement>(measurement);
        }

        public async Task<Measurement> GetMeasurement(string id)
        {
            var measurement = await _measurementRepository.FindByIdAsync(id);

            if (measurement is null)
                return null;

            return _mapper.Map<Measurement>(measurement);
        }
    }
}
