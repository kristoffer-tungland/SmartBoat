using System;
using SmartBoat.Shared.Enums;

namespace SmartBoat.Shared.Models
{
    public class Boat
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public BoatCategory Category { get; set; }
        public string Model { get; set; }
        public string Builder { get; set; }
        public double Length { get; set; }
        public double Draft { get; set; }
    }

    public class AddBoatArgs
    {
        public string Name { get; set; }
        public BoatCategory Category { get; set; }
        public string Model { get; set; }
        public string Builder { get; set; }
        public double Length { get; set; }
        public double Draft { get; set; }
    }
}
