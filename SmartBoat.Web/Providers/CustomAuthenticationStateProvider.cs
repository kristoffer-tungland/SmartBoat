using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using SmartBoat.Shared.Authentication;
using SmartBoat.Web.Services;

namespace SmartBoat.Web.Providers
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IUserService userService;

        public CustomAuthenticationStateProvider(IUserService userService)
        {
            this.userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var response = await userService.UserData(System.Threading.CancellationToken.None);

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
