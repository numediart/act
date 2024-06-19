using _Scripts._Version_1._0.Managers.Network.WebSocket.Enum;
using Newtonsoft.Json;

namespace _Scripts._Version_1._0.Managers.Network.WebSocket
{
    public class MsgFormat
    {
        public string EventName;
        public object? Data;

        public MsgFormat(string eventName, object? data)
        {
            EventName = EnumEvents.FromName(eventName).Name;
            Data = data;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class CreateGamePayload
    {
        public string? RoomId;
        public string? RoomName;
        public string RoomOwner;
        public string? Message;

        public CreateGamePayload(string roomId, string roomName, string roomOwner, string message)
        {
            RoomId = roomId;
            RoomName = roomName;
            RoomOwner = roomOwner;
            Message = message;
        }

    }
}