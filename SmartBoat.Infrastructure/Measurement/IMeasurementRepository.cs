using System.Collections.Generic;

namespace SmartBoat.Infrastructure.Measurement
{
    public interface IMeasurementRepository
    {
        List<Measurement> Measurements { get; }
    }
}