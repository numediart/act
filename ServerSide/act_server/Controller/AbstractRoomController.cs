using act_server.Service.RoomService.WoZRoomService;
using Microsoft.Extensions.Logging;

namespace act_server.Controller;

public abstract class AbstractRoomController<TRoomService, TRoom> where TRoomService : AbstractRoomService<TRoom> where TRoom : Room.Room
{

    protected abstract EventManagerService? PEventManagerService { get; set; }
    protected abstract TRoomService? PRoomService { get; set; }
    protected abstract ILogger<AbstractRoomController<TRoomService, TRoom>>? PLogger { get; set; }
    
    protected abstract void RegisterEvents();
    protected abstract void OnRequestRoomCreate(string data, string clientId);
    protected abstract void OnRequestRoomJoin(string data, string clientId);
    protected abstract void OnRequestRoomLeave(string data, string clientId);
    protected abstract void OnRequestRoomBroadcast(string data, string clientId);
    protected abstract void OnRequestPasswordChange(string data, string clientId);
    protected abstract void OnRequestRoomInfo(string data, string clientId);

}