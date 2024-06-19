using _Scripts._Version_1._0.Managers.Network.WebSocket;

namespace _Scripts._Version_1._0.Controllers.RoomController
{
    public abstract class AbstractRoomController<TController> where TController : AbstractRoomController<TController>
    {
        protected virtual EventManager EventManager { get; set; }

        protected abstract void RegisterEvents();
        protected abstract void UnregisterEvents();

        protected abstract void OnRoomCreated(string data);
        protected abstract void OnRoomJoined(string data);
        protected abstract void OnRoomLeft(string data);
        
    }
}