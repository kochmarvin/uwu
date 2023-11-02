using Microsoft.Extensions.Logging;
using netlib.client;

namespace netlib.controller;

internal class ClientController(IClient client, ILogger<ClientController> logger)
{
    public async Task StartClient(string message)
    {
        await client.SendMessage(message);
    }
}