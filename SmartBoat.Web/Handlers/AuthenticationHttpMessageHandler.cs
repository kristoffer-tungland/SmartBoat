using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using SmartBoat.Shared.Authentication;

namespace SmartBoat.Web.Handlers
{
    public class AuthenticationHttpMessageHandler : DelegatingHandler
    {

        public AuthenticationHttpMessageHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                //var token = await _protectedLocalStorage.GetAsync<string>(nameof(LoginResponse.Token));

                //if (token.Success)
                //{
                //    Console.WriteLine("Token: " + token);
                //    request.Headers.Authorization =
                //                        new AuthenticationHeaderValue("Bearer", token.Value);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine("AuthenticationHttpMessageHandler Exception: " + e.ToString());
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
