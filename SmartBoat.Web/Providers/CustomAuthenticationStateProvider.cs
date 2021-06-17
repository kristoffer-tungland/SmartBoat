using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartBoat.Shared.Authentication;
using SmartBoat.Web.Clients;
using SmartBoat.Web.Server.Extensions;
using SmartBoat.Web.Services;

namespace SmartBoat.Web.Providers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _client;

        public CustomAuthenticationStateProvider(ITokenService tokenService, ISmartBoatClient smartBoatClient)
        {
            _tokenService = tokenService;

            _client = smartBoatClient.Client;
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authentication = await _tokenService.GetAuthentication();

            if (authentication is null)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var response = await _client.GetJsonAsync<UserDataResponse>("/api/v1/User/UserData", authentication);

            var identity = GetIdentity(response);

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        private ClaimsIdentity GetIdentity(UserDataResponse response)
        {
            if (response is null)
                return new ClaimsIdentity();

            return
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, response.Name),
                new Claim(ClaimTypes.Email, response.Email),
                new Claim(ClaimTypes.GivenName, response.LastName),
            },
            "Authentication from SmartBoat API");
        }
    }
}
