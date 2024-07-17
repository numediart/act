using System;
using System.Collections.Generic;
using act_server.Controller.LiveStreamingRoomController;
using act_server.Controller.WoZRoomController;
using act_server.Room;
using act_server.Service.MainWebSocketService;
using act_server.Service.RoomService;
using act_server.Service.RoomService.WoZRoomService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace act_server
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureServices((context, services) =>
            {
                services.AddHostedService<MainWorker.MainWorker>();
                context.Configuration.GetSection("Logging").Bind(services);

                services.AddSingleton<EventManagerService>();

                services.AddSingleton(provider =>
                    new MainWebSocketService(provider.GetRequiredService<ILogger<MainWebSocketService>>(),
                        provider.GetRequiredService<EventManagerService>()));

                services.AddSingleton(provider => new WoZRoomService(
                    provider.GetRequiredService<ILogger<WoZRoom>>(),
                    provider.GetRequiredService<MainWebSocketService>()));
                services.AddSingleton(provider => new WoZRoomController(provider));

                services.AddSingleton(provider =>
                    new LiveStreamingRoomService(provider.GetRequiredService<ILogger<LiveStreamingRoom>>(),
                        provider.GetRequiredService<MainWebSocketService>()));

                services.AddSingleton(provider => new LiveStreamingRoomController(provider));


            });

            builder.ConfigureLogging(logging =>
            {
               // logging.ClearProviders();
                logging.AddConsole(options => { options.LogToStandardErrorThreshold = LogLevel.Debug; });
            });

            IHost host = builder.Build();
            host.Run();
        }
    }
}