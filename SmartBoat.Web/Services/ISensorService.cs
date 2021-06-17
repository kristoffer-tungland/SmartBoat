using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SmartBoat.Web.Services
{
    public interface ISensorService
    {
        Task EnshureConnected();
        Task Connect();
        Action<string, double> OnSensorUpdate { get; set; }
    }

    public class SensorService : ISensorService
    {
        public SensorService()
        {
            hubConnection = new HubConnectionBuilder()
            .WithUrl(new Uri("https://localhost:5001/sensorData"))
            .Build();

            hubConnection.On<string, double>("SenorUpdate", (senorId, value) =>
            {
                Console.WriteLine($"Sensor {senorId} value ={value}");
                OnSensorUpdate?.Invoke(senorId, value);
            });
        }
        private HubConnection hubConnection;

        public Action<string, double> OnSensorUpdate { get; set; }

        public async Task Connect()
        {
            Console.WriteLine("Connecting to signalR Hub");
            await hubConnection.StartAsync();

            Console.WriteLine(hubConnection.State);
        }

        public async Task EnshureConnected()
        {
            if (hubConnection.State == HubConnectionState.Disconnected)
                await Connect();
        }
    }
}
