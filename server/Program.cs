// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using netlib.controller;
using netlib.server;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

// Config files
builder.Services.Configure<ServerOptions>(builder.Configuration.GetSection("Server"));
string serverType = builder.Configuration.GetValue<string>("Type");

// Injections
builder.Services.AddSingleton<ServerController>();

switch (serverType)
{
    case "Tcp":
        builder.Services.AddTransient<IServer, TcpServer>();
        break;
    case "Udp":
        builder.Services.AddTransient<IServer, UdpServer>();
        break;
    default:
        builder.Services.AddTransient<IServer, TcpServer>();
        break;

}

using var host = builder.Build();

ServerController controller = host.Services.GetRequiredService<ServerController>();
await controller.StartServer();