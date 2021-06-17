using System;
using System.Net.Http;
using System.Threading.Tasks;
using SmartBoat.Shared.Authentication;
using SmartBoat.Web.Clients;
using System.Net.Http.Json;
using System.Threading;
using SmartBoat.Web.Providers;
using SmartBoat.Web.Server.Extensions;

namespace SmartBoat.Web.Services
{
    public interface IUserService
    {
        Task<SignUpResponse> Register(RegisterRequest model, CancellationToken cancellationToken);
        Task<LoginResponse> Login(LoginRequest model, CancellationToken cancellationToken);
        Task<UserDataResponse> UserData(CancellationToken cancellationToken);
        Task Logout();
    }

    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        private readonly HttpClient _httpClient;

        public UserService(ISmartBoatClient smartBoatClient, ITokenService tokenService, CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _tokenService = tokenService;
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
            _httpClient = smartBoatClient.Client;
        }

        public async Task<LoginResponse> Login(LoginRequest model, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/User/Login", model, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await _tokenService.SetTokenAsync(null);
                _customAuthenticationStateProvider.NotifyAuthenticationStateChanged();
                throw new Exception("Failed to log in");
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);
            
            await _tokenService.SetTokenAsync(loginResponse?.Token);
            _customAuthenticationStateProvider.NotifyAuthenticationStateChanged();

            return loginResponse;

        }

        public async Task<SignUpResponse> Register(RegisterRequest model, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostJsonAsync("/api/v1/User/Register", model, await _tokenService.GetAuthentication(),cancellationToken);

            if (!result.IsSuccessStatusCode)
                throw new Exception("Failed to register");

            return await result.Content.ReadFromJsonAsync<SignUpResponse>(cancellationToken: cancellationToken);
        }

        public async Task<UserDataResponse> UserData(CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetJsonAsync<UserDataResponse>("/api/v1/User/UserData", await _tokenService.GetAuthentication(), cancellationToken);
        }

        public async Task Logout()
        {
            await _tokenService.SetTokenAsync(null);
            _customAuthenticationStateProvider.NotifyAuthenticationStateChanged();
        }
    }
}
