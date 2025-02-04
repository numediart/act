using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace act_server.Controller
{
    public struct RoomCreationData
    {
        public readonly string? RoomName;
        public readonly string? RoomOwner;
        public readonly string? Password;

        public RoomCreationData(string? roomName, string? roomOwner, string? password)
        {
            RoomName = roomName;
            RoomOwner = roomOwner ?? null;
            Password = password;
        }
    }
    public abstract class AbstractRoomService<TRoom>
        where TRoom : Room.Room
    {
        public AbstractRoomService<TRoom>(ILogger<TRoom> logger) {
            Logger = logger;
        }

        protected abstract ILogger<TRoom> Logger { get; set; }

        protected abstract Dictionary<string, TRoom> Rooms { get; set; }
        public abstract void OnRequestRoomCreate(RoomCreationData roomCreationData);
        public abstract void OnRequestRoomJoin(string roomId, string clientId, string? password = null);
        public abstract void OnRequestRoomLeave(string roomId, string clientId);
        public abstract void OnRequestRoomBroadcast(string roomId, string message);

        public abstract void OnRequestPasswordChange(string roomId, string clientId, string newPassword);
        public abstract void OnRequestRoomInfo(string roomId, string clientId);

    }
}