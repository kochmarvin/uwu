using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace netlib.server;

internal class TcpServer(IOptions<ServerOptions> options, ILogger<TcpServer> logger) : IServer
{
    private TcpListener listener;

    public async Task Start()
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, options.Value.Port);
            listener.Start();

            logger.LogInformation($"Server is listeneing on {options.Value.Port}");
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                logger.LogInformation($"Client connected {((IPEndPoint)client.Client.RemoteEndPoint).Address}");
                _ = Task.Run(() => HandleClient(client));
            }
        }
        catch (Exception ex)
        {
        }
    }
    public async Task HandleClient(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                logger.LogInformation($"Got Message {Encoding.UTF8.GetString(buffer, 0, bytesRead)}");
                await stream.WriteAsync(buffer, 0, bytesRead);
            }
        }
        client.Close();
    }

}