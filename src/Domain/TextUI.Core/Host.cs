using Application.TextCommunication.Core.Interfaces;
using Application.TextCommunication.Core.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Application.TextCommunication.Core;

public class Host : IHost
{
    public Host(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        RoutingTable routingTable
        )
    {
        Configuration = configuration;
        Container = serviceProvider;
        RoutingTable = routingTable;
    }
    public IConfiguration Configuration { get; init; }
    public IServiceProvider Container { get; init; }
    public RoutingTable RoutingTable { get; init; }
    private bool _started = false;

    public async Task Start()
    {
        if (_started)
            throw new Exception("The host is already started");
        _started = true;
        var handler = Container.GetService<IMessageHandler>();
        await handler.StartReceiving();
    }

}