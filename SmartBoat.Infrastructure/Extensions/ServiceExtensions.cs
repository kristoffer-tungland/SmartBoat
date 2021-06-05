using System;
using Microsoft.Extensions.DependencyInjection;
using SmartBoat.Infrastructure.Measurement;

namespace SmartBoat.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSmartBoatServices(this IServiceCollection services)
        {
            services
                .AddScoped<IMeasurementService, MeasurementService>()
                .AddSingleton<IMeasurementRepository, MeasurementRepository>()
                ;
        }
    }
}
