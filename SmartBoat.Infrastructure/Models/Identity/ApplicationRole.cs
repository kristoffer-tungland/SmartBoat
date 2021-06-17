using System;
using AspNetCore.Identity.Mongo.Model;

namespace SmartBoat.Infrastructure.Models.Identity
{
    public class ApplicationRole : MongoRole
    {
        
    }

    public class ApplicationUser : MongoUser
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime? Birthdate { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }
    }
}
