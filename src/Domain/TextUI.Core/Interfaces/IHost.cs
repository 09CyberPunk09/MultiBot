using System;
using System.Threading.Tasks;

namespace Application.Chatting.Core.Interfaces;

public interface IHost
{
    IServiceProvider Container { get; init; }
    Task Start();
}
