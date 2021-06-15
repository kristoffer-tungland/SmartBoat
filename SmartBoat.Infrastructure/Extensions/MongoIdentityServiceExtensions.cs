using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartBoat.Infrastructure.Models.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using AspNetCore.Identity.Mongo;
using SmartBoat.Infrastructure.Settings;

namespace SmartBoat.Infrastructure.Extensions
{
    public static class MongoIdentityServiceExtensions
    {
        public static void AddMongoIdentity(this IServiceCollection services, MongoDbSettings mongoDbSettings)
        {
            // Configure Identity MongoDB
            services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>
            (identity =>
            {
                identity.Password.RequiredLength = 6;
                identity.Password.RequireLowercase = true;
                identity.Password.RequireUppercase = true;
                identity.Password.RequireNonAlphanumeric = true;
                identity.Password.RequireDigit = true;
            },
            mongo =>
            {
                mongo.ConnectionString = mongoDbSettings.ConnectionString;
            });

            // Add Jwt Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(options =>
            {
                //Set default Authentication Schema as Bearer
                options.DefaultAuthenticateScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters =
                       new TokenValidationParameters
                       {
                           ValidIssuer = mongoDbSettings.JwtIssuer,
                           ValidAudience = mongoDbSettings.JwtIssuer,
                           IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mongoDbSettings.JwtKey)),
                           ClockSkew = TimeSpan.Zero // remove delay of token when expire
                       };
            });
        }
    }
}
