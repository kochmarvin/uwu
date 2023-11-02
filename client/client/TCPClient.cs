using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace netlib.client;

internal class TCPClient(IOptions<ClientOptions> options, ILogger<TCPClient> logger) : IClient
{
    public async Task SendMessage(string message)
    {
        TcpClient client = new TcpClient();
        await client.ConnectAsync(options.Value.Hostname, options.Value.Port);

        logger.LogInformation($"Connected to server {options.Value.Hostname}:{options.Value.Port}");

        using (NetworkStream stream = client.GetStream())
        {

            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);


            logger.LogInformation($"Got Message {response}");
        }

        client.Close();
    }

}