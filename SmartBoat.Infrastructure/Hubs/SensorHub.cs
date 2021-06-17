using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SmartBoat.Infrastructure.Hubs
{
    public class SensorHub : Hub
    {
        public async Task SenorUpdate(string sensorId, double value)
        {
            await Clients.All.SendAsync(nameof(SenorUpdate), sensorId, value);
        }
    }
}
