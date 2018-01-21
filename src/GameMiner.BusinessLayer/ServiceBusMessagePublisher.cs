using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer
{
    public class ServiceBusMessagePublisher : IMessagePublisher
    {
        private string _connectionString;
        private readonly ILogger _logger;

        public ServiceBusMessagePublisher(IConfiguration configuration, ILogger<ServiceBusMessagePublisher> logger)
            : this(configuration.GetConnectionString("ServiceBusConnection"))
        {
            _logger = logger;
        }

        protected ServiceBusMessagePublisher(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Publish(string message, string queue, DateTime enqueueTime)
        {
            var queueClient = new QueueClient(_connectionString, queue);

            queueClient.ScheduleMessageAsync(new Message(Encoding.UTF8.GetBytes(message)), enqueueTime);
        }
    }
}
