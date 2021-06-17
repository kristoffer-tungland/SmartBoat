using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SmartBoat.Shared.Models;

namespace SmartBoat.Web.Clients
{
    public interface ISmartBoatClient
    {
        HttpClient Client { get; }

        Task<Measurement> GetMeasurement();
    }

    public class SmartBoatClient : ISmartBoatClient
    {
        public HttpClient Client { get; }

        public SmartBoatClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "SmartBoat.Web");

            Client = client;
        }

        public async Task<Measurement> GetMeasurement()
        {
            var response = await Client.GetFromJsonAsync<List<Measurement>>("Measurement");
            return response.LastOrDefault();
        }
    }
}
