using System;
using RabbitMQ.Client;

namespace ServiceBus
{
    public interface IRabbitMqConnection:IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();
    }
}