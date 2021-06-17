using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SmartBoat.Shared.Authentication;

namespace SmartBoat.Web.Handlers
{
    public class AuthenticationHttpMessageHandler : DelegatingHandler
    {
        private readonly ProtectedLocalStorage protectedLocalStorage;

        public AuthenticationHttpMessageHandler(ProtectedLocalStorage protectedLocalStorage)
        {
            this.protectedLocalStorage = protectedLocalStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                var token = await protectedLocalStorage.GetAsync<string>(nameof(LoginResponse.Token));

                if (token.Success)
                {
                    Console.WriteLine("Token: " + token);
                    request.Headers.Authorization =
                                        new AuthenticationHeaderValue("Bearer", token.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AuthenticationHttpMessageHandler Exception: " + e.ToString());
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
