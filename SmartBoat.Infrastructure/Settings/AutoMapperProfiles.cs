using System;
using AutoMapper;
using MongoDB.Bson;
using SmartBoat.Infrastructure.Converters;
using SmartBoat.Infrastructure.Models;
using SmartBoat.Infrastructure.Services;
using SmartBoat.Shared.Models;

namespace SmartBoat.Infrastructure.Settings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ObjectId, string>().ConvertUsing<ObjectIdToStringConverter>();
            CreateMap<string, ObjectId>().ConvertUsing<StringToObjectIdConverter>();

            CreateMap<MeasurementStorage, Measurement>().ReverseMap();
            CreateMap<AddMeasurementArgs, MeasurementStorage>();

            CreateMap<BoatStorage, Boat>().ReverseMap();
            CreateMap<AddBoatArgs, BoatStorage>();
        }
    }
}
