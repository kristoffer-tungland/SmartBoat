using AutoMapper;
using MongoDB.Bson;

namespace SmartBoat.Infrastructure.Converters
{
    internal class ObjectIdToStringConverter : ITypeConverter<ObjectId, string>
    {
        public string Convert(ObjectId source, string destination, ResolutionContext context)
        {
            return source.ToString();
        }
    }
}