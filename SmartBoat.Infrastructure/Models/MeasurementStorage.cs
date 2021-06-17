using SmartBoat.Infrastructure.Attributes;

namespace SmartBoat.Infrastructure.Models
{
    [BsonCollection("measurements")]
    public class MeasurementStorage : Document
    {
        public double Value { get; set; }

    }
}
