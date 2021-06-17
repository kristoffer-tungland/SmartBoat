using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmartBoat.Web.Server.Extensions
{
    public static class ServiceExtensions
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string url, AuthenticationHeaderValue authorization, CancellationToken cancellationToken = default)
        {
            httpClient.DefaultRequestHeaders.Authorization = authorization;
            return await httpClient.GetFromJsonAsync<T>(url, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, string url, T model, AuthenticationHeaderValue authorization, CancellationToken cancellationToken = default)
        {
            httpClient.DefaultRequestHeaders.Authorization = authorization;
            return await httpClient.PostAsJsonAsync(url, model, cancellationToken);
        }
    }
}