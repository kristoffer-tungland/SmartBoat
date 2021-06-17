using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SmartBoat.Shared.Authentication;

namespace SmartBoat.Web.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync();
        Task SetTokenAsync(string token);
        Task<AuthenticationHeaderValue> GetAuthentication();
    }

    internal class TokenService : ITokenService
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        public TokenService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                var token = await _protectedLocalStorage.GetAsync<string>(nameof(LoginResponse.Token));

                return token.Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }


        public async Task SetTokenAsync(string token)
        {
            try
            {
                if (token is null)
                {
                    await _protectedLocalStorage.DeleteAsync(nameof(LoginResponse.Token));
                }
                else
                {
                    await _protectedLocalStorage.SetAsync(nameof(LoginResponse.Token), token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<AuthenticationHeaderValue> GetAuthentication()
        {
            var token = await GetTokenAsync();

            return token is null ? null : new AuthenticationHeaderValue("Bearer", token);
        }
    }
}