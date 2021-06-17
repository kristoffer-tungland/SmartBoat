using AutoMapper;
using MongoDB.Bson;

namespace SmartBoat.Infrastructure.Converters
{
    internal class StringToObjectIdConverter : ITypeConverter<string, ObjectId>
    {
        public ObjectId Convert(string source, ObjectId destination, ResolutionContext context)
        {
            destination = new ObjectId(source);

            return destination;
        }
    }
}