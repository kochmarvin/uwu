
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace netlib.server;

internal class UdpServer(IOptions<ServerOptions> options, ILogger<UdpServer> logger) : IServer
{
    private UdpClient server;
    public async Task Start()
    {
        server = new UdpClient(options.Value.Port);

        logger.LogInformation($"Server is listeneing on {options.Value.Port}");

        while (true)
        {
            UdpReceiveResult result = await server.ReceiveAsync();
            byte[] receivedData = result.Buffer;
            IPEndPoint clientEndPoint = result.RemoteEndPoint;

            string message = Encoding.UTF8.GetString(receivedData);
            logger.LogInformation($"Server is listeneing on {clientEndPoint}: {message}");

            await server.SendAsync(receivedData, receivedData.Length, clientEndPoint);
        }
    }
}