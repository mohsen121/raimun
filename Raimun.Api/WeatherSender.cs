using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Raimun.Api.Configurations;
using Raimun.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raimun.Api
{
    public class WeatherSender
    {
        private RabbitMQConfig _rabbitConfig;

        public WeatherSender(IOptionsMonitor<RabbitMQConfig> optionsMonitor)
        {
            _rabbitConfig = optionsMonitor.CurrentValue;
        }

        public void Send(WeatherRequestModel model)
        {
            var factory = new ConnectionFactory() { HostName = _rabbitConfig.Hostname, UserName = _rabbitConfig.UserName, Password = _rabbitConfig.Password };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var json = JsonConvert.SerializeObject(model);
                var body = Encoding.UTF8.GetBytes(json);

                //channel.QueueDeclare(_rabbitConfig.QueueName, durable: false, exclusive: false);

                channel.BasicPublish(exchange: "", routingKey: _rabbitConfig.QueueName, basicProperties: null, body: body);
            }
        }
    }
}
