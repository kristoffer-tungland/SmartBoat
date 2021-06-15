using SmartBoat.Infrastructure.Attributes;
using SmartBoat.Shared.Enums;

namespace SmartBoat.Infrastructure.Models
{
    [BsonCollection("boats")]
    public class BoatStorage : Document
    {
        public string Name { get; set; }
        public BoatCategory Category { get; set; }
        public string Model { get; set; }
        public string Builder { get; set; }
        public double Length { get; set; }
        public double Draft { get; set; }
    }
}
