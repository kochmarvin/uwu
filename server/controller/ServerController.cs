using Microsoft.Extensions.Logging;
using netlib.server;

namespace netlib.controller;

internal class ServerController(IServer server, ILogger<ServerController> logger)
{
    public async Task StartServer()
    {
        await server.Start();
    }
}