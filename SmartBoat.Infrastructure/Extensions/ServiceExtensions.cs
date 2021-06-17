using Microsoft.Extensions.DependencyInjection;
using SmartBoat.Infrastructure.Repository;
using SmartBoat.Infrastructure.Services;
using SmartBoat.Infrastructure.Services.Users;
using SmartBoat.Infrastructure.Settings;

namespace SmartBoat.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSmartBoatServices(this IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(AutoMapperProfiles))
                .AddTransient<IMeasurementService, MeasurementService>()
                .AddTransient<IBoatService, BoatService>()
                .AddTransient<IUserService, UserService>()
                .AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>))
                ;
        }
    }
}
