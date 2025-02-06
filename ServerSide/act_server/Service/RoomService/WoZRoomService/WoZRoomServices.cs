using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using act_server.Controller;
using act_server.DataDescriptorClass;
using act_server.Enum;
using act_server.Room;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace act_server.Service.RoomService.WoZRoomService
{

    public struct AvatarHeadMoveData
    {
        public double x;
        public double y;
        public double z;
    }

    public struct AvatarBlendshapeMoveData
    {
        public Dictionary<string, double> BlendshapeDict;
    }

    public struct AvatarBlendshapeTransitionData
    {
        public Dictionary<string, double> BlendshapeDict;
        public string Value;
        public float Duration;

        public AvatarBlendshapeTransitionData(Dictionary<string, double> blendshapeDict, string value, float duration)
        {
            BlendshapeDict = blendshapeDict;
            Value = value;
            Duration = duration;
        }
    }

    public struct AvatarPoseTransitionData
    {
        public double x;
        public double y;
        public double z;
        public string Duration;
    }

    /// <summary>
    /// Class to control the WoZRoom, and process WoZRoom events
    /// </summary>
    public class WoZRoomService : AbstractRoomService<WoZRoom>
    {
        public WoZRoomService(ILogger<WoZRoom> logger, MainWebSocketService.MainWebSocketService mainWebSocketService) : base(logger)
        {
            Logger = logger;
            _mainWebSocketService = mainWebSocketService;
        }
        protected override ILogger<WoZRoom> Logger { get; set; }
        protected MainWebSocketService.MainWebSocketService _mainWebSocketService;
        protected sealed override Dictionary<string, WoZRoom> Rooms { get; set; } = new Dictionary<string, WoZRoom>();

        public override void OnRequestRoomCreate(RoomCreationData roomCreationData)
        {
            try
            {
                WoZRoom room = new WoZRoom(Logger);
                Logger.LogInformation("Creating room with id: " + room.RoomId + " and owner: " +
                                      roomCreationData.RoomOwner +
                                      " and name: " + roomCreationData.RoomName + " and password: " +
                                      roomCreationData.Password);
                room.InitRoom(roomCreationData.RoomOwner!, roomCreationData.RoomName, roomCreationData.Password);
                RoomInfo roomInfo = new RoomInfo(room.RoomName, room.RoomOwner, room.RoomId.ToString(), room.HasPassword(),
                    room.Clients.Count);
                room.AddClient(_mainWebSocketService.GetClient(room.RoomOwner));
                Rooms.Add(room.RoomId.ToString(), room);
                _mainWebSocketService.GetClient(room.RoomOwner).Emit(JsonConvert.SerializeObject(new
                { EventName = EnumEvents.WoZRoomCreated.Name, Data = roomInfo.ToJson() }));
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
        }

        public override void OnRequestRoomJoin(string roomId, string clientId, string? password = null)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                if (password != null && !room.VerifyPassword(password))
                {
                    Logger.LogError("Password incorrect");
                    return;
                }

                room.AddClient(_mainWebSocketService.GetClient(clientId));

                _mainWebSocketService.GetClient(clientId).Emit(EnumEvents.WoZRoomJoined.Name, room.RoomId.ToString());
            }
        }

        public override void OnRequestRoomLeave(string roomId, string clientId)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                Client.Client client = _mainWebSocketService.GetClient(clientId);
                if (client.RoomId.ToString() != roomId)
                {
                    Logger.LogError("Client not in room");
                    return;
                }

                room.RemoveClient(_mainWebSocketService.GetClient(clientId));
                _mainWebSocketService.GetClient(clientId).Emit(EnumEvents.WoZRoomLeft.Name, room.RoomId.ToString());
            }
        }

        public override void OnRequestRoomBroadcast(string roomId, string message)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                room.BroadcastToRoom(message);
            }
        }

        public override void OnRequestPasswordChange(string roomId, string clientId, string newPassword)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                if (room.CheckAdmin(clientId))
                {
                    room.ChangePassword(newPassword);
                    // mainWebSocketService.GetClient(clientId).Emit(EnumEvents.WoZRoomPasswordChanged.Name, room.RoomId.ToString());
                    return;
                }

                Logger.LogError("Client is not admin, Action denied");
            }
        }

        public override void OnRequestRoomInfo(string roomId, string clientId)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                _mainWebSocketService.GetClient(clientId).Emit("RoomInfo", room.ToString());
            }
        }

        public void OnRequestWoZRoomInfos(string clientId)
        {
            List<object> roomInfos = new List<object>();
            if (Rooms.Values.Count == 0)
            {
                _mainWebSocketService.GetClient(clientId).Emit(EnumEvents.WoZRoomsInfos.Name, roomInfos);
                return;
            }

            foreach (WoZRoom room in Rooms.Values)
            {
                roomInfos.Add(new RoomInfo(room.RoomName, room.RoomOwner, room.RoomId.ToString(), room.HasPassword(),
                    room.Clients.Count).ToJson());
            }

            _mainWebSocketService.GetClient(clientId).Emit(EnumEvents.WoZRoomsInfos.Name, roomInfos);
        }

        public void OnRequestAvatarHeadMove(string roomId, string clientId, AvatarHeadMoveData data)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                if (room.CheckAdmin(clientId))
                {
                    foreach (Client.Client client in room.Clients)
                    {
                        if (client.ClientId.ToString() != clientId)
                            room.Emit(EnumEvents.AvatarHeadMove.Name,
                                JsonConvert.SerializeObject(new { x = data.x, y = data.y, z = data.z }),
                                client.ClientId.ToString());
                    }

                    return;
                }

                Logger.LogError("Client is not admin, Action denied");
            }
        }

        public void OnRequestAvatarBlendshapeMove(string roomId, string clientId,
            AvatarBlendshapeMoveData avatarBlendshapeMoveData)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                if (room.CheckAdmin(clientId))
                {
                    foreach (Client.Client client in room.Clients)
                    {
                        if (client.ClientId.ToString() != clientId)
                            room.Emit(EnumEvents.AvatarBlendshapeMove.Name,
                                JsonConvert.SerializeObject(
                                    new { BlendshapeDict = avatarBlendshapeMoveData.BlendshapeDict }),
                                client.ClientId.ToString());
                    }

                    return;
                }

                Logger.LogError("Client is not admin, Action denied");
            }
        }

        public void OnRequestAvatarBlendshapeTransition(string roomId, string clientId,
            AvatarBlendshapeTransitionData blendshapeTransitionData)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                if (room.CheckAdmin(clientId))
                {
                    foreach (Client.Client client in room.Clients)
                    {
                        if (client.ClientId.ToString() != clientId)
                            room.Emit(EnumEvents.AvatarBlendshapeTransition.Name,
                                JsonConvert.SerializeObject(new
                                {
                                    blendshapeDict = blendshapeTransitionData.BlendshapeDict,
                                    duration = blendshapeTransitionData.Duration
                                }),
                                client.ClientId.ToString());
                    }

                    return;
                }

                Logger.LogError("Client is not admin, Action denied");
            }
        }

        public void OnRequestAvatarPoseTransition(string roomId, string clientId,
            AvatarPoseTransitionData poseTransitionData)
        {
            if (Rooms.TryGetValue(roomId, out WoZRoom room))
            {
                if (room.CheckAdmin(clientId))
                {
                    foreach (Client.Client client in room.Clients)
                    {
                        if (client.ClientId.ToString() != clientId)
                            room.Emit(EnumEvents.AvatarPoseTransition.Name,
                                JsonConvert.SerializeObject(new
                                {
                                    x = poseTransitionData.x,
                                    y = poseTransitionData.y,
                                    z = poseTransitionData.z,
                                    Duration = poseTransitionData.Duration
                                }),
                                client.ClientId.ToString());
                    }

                    return;
                }

                Logger.LogError("Client is not admin, Action denied");
            }
        }
    }
}