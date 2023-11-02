using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using netlib.client;

internal class UDPClient(IOptions<ClientOptions> options, ILogger<UDPClient> logger) : IClient
{
    public async Task SendMessage(string message)
    {
        UdpClient client = new UdpClient();
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(options.Value.Hostname), options.Value.Port);

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(data, data.Length, serverEndPoint);

            UdpReceiveResult result = await client.ReceiveAsync();
            byte[] receivedData = result.Buffer;
            string response = Encoding.UTF8.GetString(receivedData);
            logger.LogInformation($"Got Message {response}");

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}