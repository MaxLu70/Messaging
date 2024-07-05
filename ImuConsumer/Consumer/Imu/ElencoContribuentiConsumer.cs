using MassTransit;
using MessageContracts.IMU.Requests;
using MessageContracts.IMU.Responses;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Repository;
using Azure;

namespace ImuConsumer.Consumer.Imu
{
    public class ElencoContribuentiConsumer : BackgroundService
    {
        private readonly IBusControl bus;
        private readonly ImuRepository repository = new();
        public ElencoContribuentiConsumer()
        {
            bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost"), "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("imu-service", e =>
                {
                    e.Handler<IContribuentiRequest>(context =>
                    {
                        var t = repository.FetchContribuenti(context.Message);
                        Task.WaitAll(t);
                        var list = t.Result;
                        var response = new { Contribuenti = list };
                        return context.RespondAsync<IContribuentiResponse>(response);
                    });
                });
            });
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return bus.StartAsync(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(base.StopAsync(cancellationToken), bus.StopAsync(cancellationToken));
        }
    }
}
