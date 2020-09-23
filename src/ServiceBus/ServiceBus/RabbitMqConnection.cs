using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace ServiceBus
{
    public class RabbitMqConnection:IRabbitMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;
        

        public RabbitMqConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if (!IsConnected) TryConnect();
        }
        public void Dispose()
        {
            if (_disposed)
                return;
            
            _connection.Dispose();
        }

        public bool IsConnected => _connection != null && _connection.IsOpen &&  !_disposed ;
        
        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Thread.Sleep(2000);
                TryConnect();
            }
            return IsConnected;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Connection to messaging queue is not open");
            }
            return _connection.CreateModel();
        }
    }
}