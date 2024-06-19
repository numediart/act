

using System.Threading;
using System.Threading.Tasks;
using act_server.Controller.LiveStreamingRoomController;
using act_server.Controller.WoZRoomController;
using act_server.Service.MainWebSocketService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace act_server.MainWorker
{
    public class MainWorker: BackgroundService
    {
        private readonly MainWebSocketService _mainWebSocketService;
        private readonly ILogger<MainWorker> _logger;
        private readonly EventManagerService _eventManagerService;
        private readonly LiveStreamingRoomController _liveStreamingRoomController;
        private readonly WoZRoomController _woZRoomController;
        public MainWorker(MainWebSocketService mainWebSocketService,  EventManagerService eventManagerService,ILogger<MainWorker> logger, LiveStreamingRoomController liveStreamingRoomController, WoZRoomController woZRoomController)
        {
            _mainWebSocketService = mainWebSocketService;
            _eventManagerService = eventManagerService;
            _logger = logger;
            _liveStreamingRoomController = liveStreamingRoomController;
            _woZRoomController = woZRoomController;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}