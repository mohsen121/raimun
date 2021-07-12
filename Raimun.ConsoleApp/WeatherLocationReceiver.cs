using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Raimun.ConsoleApp.Configurations;
using Raimun.Core.Common.Interfaces;
using Raimun.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raimun.ConsoleApp
{
    public class WeatherLocationReceiver : BackgroundService
    {
        private RabbitMQConfig _rabbitConfig;
        private IWeatherService _weatherService;
        private IConnection _connection;
        private IModel _channel;

        public WeatherLocationReceiver(IOptionsMonitor<RabbitMQConfig> optionsMonitor, IWeatherService weatherService)
        {
            _rabbitConfig = optionsMonitor.CurrentValue;
            _weatherService = weatherService;
            InitializeRabbitMqListener();
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var weatherModel = JsonConvert.DeserializeObject<WeatherRequestModel>(content);


                await _weatherService.HandleLocationWeatherWithGeo(weatherModel.Lat, weatherModel.Lon, weatherModel.Date);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            //consumer.Shutdown += OnConsumerShutdown;
            //consumer.Registered += OnConsumerRegistered;
            //consumer.Unregistered += OnConsumerUnregistered;
            //consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_rabbitConfig.QueueName, false, consumer);
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitConfig.Hostname,
                UserName = _rabbitConfig.UserName,
                Password = _rabbitConfig.Password
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _rabbitConfig.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
