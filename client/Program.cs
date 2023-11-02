using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using netlib.client;
using netlib.controller;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

// Config files
builder.Services.Configure<ClientOptions>(builder.Configuration.GetSection("Server"));
string clientType = builder.Configuration.GetValue<string>("Type");


// Injections
builder.Services.AddSingleton<ClientController>();

switch (clientType)
{
    case "Tcp":
        builder.Services.AddTransient<IClient, TCPClient>();
        break;
    case "Udp":
        builder.Services.AddTransient<IClient, UDPClient>();
        break;
    default:
        builder.Services.AddTransient<IClient, TCPClient>();
        break;

}

using var host = builder.Build();

ClientController controller = host.Services.GetRequiredService<ClientController>();
await controller.StartClient("saseeeerrr");