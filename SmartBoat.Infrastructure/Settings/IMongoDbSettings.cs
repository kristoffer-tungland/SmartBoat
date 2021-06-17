namespace SmartBoat.Infrastructure.Settings
{
    public interface IMongoDbSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtKey { get; set; }
        public int JwtExpireDays { get; set; }
    }

    public class MongoDbSettings : IMongoDbSettings
    {
        public static string Field => "MongoDbSettings";
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtKey { get; set; }
        public int JwtExpireDays { get; set; }
    }
}
