using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SmartBoat.Infrastructure.Models;
using SmartBoat.Infrastructure.Repository;
using SmartBoat.Shared.Models;

namespace SmartBoat.Infrastructure.Services
{
    public interface IBoatService
    {
        Task<Boat> AddBoat(AddBoatArgs args);
        Task<Boat> GetBoat(string id);
        Task<List<Boat>> GetBoats();
        Task DeleteBoat(string id);
        Task<Boat> UpdateBoat(Boat boat);
    }

    internal class BoatService : IBoatService
    {
        private readonly IMongoRepository<BoatStorage> _boatRepository;
        private readonly IMapper _mapper;

        public BoatService(IMongoRepository<BoatStorage> boatRepository, IMapper mapper)
        {
            _boatRepository = boatRepository;
            _mapper = mapper;
        }

        public async Task<List<Boat>> GetBoats()
        {
            await Task.Delay(0);
            var items = _boatRepository.FilterBy(x => true);

            return _mapper.Map<List<Boat>>(items);
        }

        public async Task<Boat> AddBoat(AddBoatArgs args)
        {
            var boat = _mapper.Map<BoatStorage>(args);

            await _boatRepository.InsertOneAsync(boat);

            return _mapper.Map<Boat>(boat);
        }

        public async Task<Boat> GetBoat(string id)
        {
            var boat = await _boatRepository.FindByIdAsync(id);

            if (boat is null)
                return null;

            return _mapper.Map<Boat>(boat);
        }

        public Task DeleteBoat(string id)
        {
            return _boatRepository.DeleteByIdAsync(id);
        }

        public async Task<Boat> UpdateBoat(Boat boat)
        {
            var boatStorage = _mapper.Map<BoatStorage>(boat);

            await _boatRepository.ReplaceOneAsync(boatStorage);

            return _mapper.Map<Boat>(boatStorage);
        }
    }
}
