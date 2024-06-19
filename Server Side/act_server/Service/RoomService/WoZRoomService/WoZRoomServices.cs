using System;
using System.Collections.Generic;
using act_server.Controller;
using act_server.Room;
using Microsoft.Extensions.Logging;

namespace act_server.Service.RoomService.WoZRoomService;

public record struct AvatarHeadMoveData
{
    public string X;
    public string Y;
    public string Z;
}

public record struct AvatarBlendshapeMoveData
{
    public string BlendshapeName;
    public string Value;
}

public record struct AvatarBlendshapeTransitionData
{
    public string BlendshapeName;
    public string Value;
    public string Duration;
}

public struct AvatarPoseTransitionData
{
    public string PoseName;
    public string Duration;
}

/// <summary>
/// Class to control the WoZRoom, and process WoZRoom events
/// </summary>
public class WoZRoomService(ILogger<WoZRoom> logger, MainWebSocketService.MainWebSocketService mainWebSocketService)
    : AbstractRoomService<WoZRoom>(logger)
{
    protected override ILogger<WoZRoom> Logger {get; set;} = logger;
    protected sealed override Dictionary<string, WoZRoom> Rooms { get; set; } = new Dictionary<string, WoZRoom>();
    
    public override void OnRequestRoomCreate(RoomCreationData roomCreationData)
    {
        WoZRoom room = new WoZRoom(Logger);
        room.InitRoom(roomCreationData.RoomOwner, roomCreationData.RoomName, roomCreationData.Password);
        try
        {
            Rooms.Add(room.RoomId.ToString(), room);
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

            room.AddClient(mainWebSocketService.GetClient(clientId));
        }
    }

    public override void OnRequestRoomLeave(string roomId, string clientId)
    {
        if (Rooms.TryGetValue(roomId, out WoZRoom room))
        {
            Client.Client client = mainWebSocketService.GetClient(clientId);
            if (client.RoomId.ToString() != roomId)
            {
                Logger.LogError("Client not in room");
                return;
            }

            room.RemoveClient(mainWebSocketService.GetClient(clientId));
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
                return;
            }

            Logger.LogError("Client is not admin, Action denied");
        }
    }

    public override void OnRequestRoomInfo(string roomId, string clientId)
    {
        if (Rooms.TryGetValue(roomId, out WoZRoom room))
        {
            mainWebSocketService.GetClient(clientId).Emit("RoomInfo", room.ToString());
        }
    }

    public void OnRequestAvatarHeadMove(string roomId, string clientId, AvatarHeadMoveData data)
    {
        if (Rooms.TryGetValue(roomId, out WoZRoom room))
        {
            if (room.CheckAdmin(clientId))
            {
                foreach (Client.Client client in room.Clients)
                {
                    room.Emit("AvatarHeadMove", data.ToString(), client.ClientId.ToString());
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
                    room.Emit("AvatarBlendshapeMove",
                        $"{{\"blendshapeName\":\"{avatarBlendshapeMoveData.BlendshapeName}\",\"value\":\"{avatarBlendshapeMoveData.Value}\"}}",
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
                    room.Emit("AvatarBlendshapeTransition",
                        $"{{\"blendshapeName\":\"{blendshapeTransitionData.BlendshapeName}\",\"value\":\"{blendshapeTransitionData.Value}\",\"duration\":\"{blendshapeTransitionData.Duration}\"}}",
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
                    room.Emit("AvatarPoseTransition",
                        $"{{\"poseName\":\"{poseTransitionData.PoseName}\",\"duration\":\"{poseTransitionData.Duration}\"}}",
                        client.ClientId.ToString());
                }

                return;
            }

            Logger.LogError("Client is not admin, Action denied");
        }
    }
}