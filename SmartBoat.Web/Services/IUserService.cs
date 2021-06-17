using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SmartBoat.Shared.Authentication;
using SmartBoat.Web.Clients;
using System.Net.Http.Json;
using System.Threading;

namespace SmartBoat.Web.Services
{
    public interface IUserService
    {
        Task<SignUpResponse> Register(RegisterRequest model, CancellationToken cancellationToken);
        Task<LoginResponse> Login(LoginRequest model, CancellationToken cancellationToken);
        Task<UserDataResponse> UserData(CancellationToken cancellationToken);
    }

    public class UserService : IUserService
    {
        private readonly ProtectedLocalStorage protectedLocalStorage;
        private readonly HttpClient _httpClient;

        public UserService(ProtectedLocalStorage protectedLocalStorage, ISmartBoatClient smartBoatClient)
        {
            this.protectedLocalStorage = protectedLocalStorage;
            _httpClient = smartBoatClient.Client;
        }

        public async Task<LoginResponse> Login(LoginRequest model, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/User/Login", model, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to log in");

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);

            await protectedLocalStorage.SetAsync(nameof(LoginResponse.Token), loginResponse.Token);

            return loginResponse;
        }

        public async Task<SignUpResponse> Register(RegisterRequest model, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/v1/User/Register", model, cancellationToken);

            if (!result.IsSuccessStatusCode)
                throw new Exception("Failed to register");

            return await result.Content.ReadFromJsonAsync<SignUpResponse>(cancellationToken: cancellationToken);
        }

        public async Task<UserDataResponse> UserData(CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<UserDataResponse>("/api/v1/User/UserData", cancellationToken);
        }
    }
}
