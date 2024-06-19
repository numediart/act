using System.Collections.Generic;
using act_server.Behavior;
using act_server.Controller;
using act_server.DataDescriptorClass;
using act_server.Enum;
using act_server.Room;
using act_server.Utils;
using act_server.Utils.AU2BlendShapes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace act_server.Service.RoomService;

public class LiveStreamingRoomService(ILogger<LiveStreamingRoom> logger, MainWebSocketService.MainWebSocketService mainWebSocketService)
    : AbstractRoomService<LiveStreamingRoom>(logger)
{
    protected override ILogger<LiveStreamingRoom> Logger { get; set; } = logger;
    private MainWebSocketService.MainWebSocketService _mainWebSocketService = mainWebSocketService;

    protected sealed override Dictionary<string, LiveStreamingRoom> Rooms { get; set; } =
        new Dictionary<string, LiveStreamingRoom>();

    public override void OnRequestRoomCreate(RoomCreationData roomCreationData)
    {
        logger.LogInformation("Creating room");
        LiveStreamingRoom room = new LiveStreamingRoom(Logger);
        _mainWebSocketService.RegisterWebsocketService<ConnectionBehavior>("/openface/ActionUnit/" + room.RoomId);
        _mainWebSocketService.RegisterWebsocketService<ConnectionBehavior>("/openface/AudioData/" + room.RoomId);
        logger.LogInformation("Room created with id: " + room.RoomId);
        room.InitRoom(roomCreationData.RoomOwner ?? "", roomCreationData.RoomName, roomCreationData.Password);
        try
        {   
            Rooms.Add(room.RoomId.ToString(), room);
            room.AddClient(mainWebSocketService.GetClient(roomCreationData.RoomOwner!));
        }
        catch (System.Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }

    public override void OnRequestRoomJoin(string roomId, string clientId, string? password = null)
    {
        if (Rooms.TryGetValue(roomId, out LiveStreamingRoom room))
        {
            if (room.VerifyPassword(password))
            {
                room.AddClient(mainWebSocketService.GetClient(clientId));
            }
            else
            {
                Logger.LogError("Password incorrect");
            }
        }
        else
        {
            Logger.LogError("Room not found");
        }
    }

    public override void OnRequestRoomLeave(string roomId, string clientId)
    {
        if (Rooms.TryGetValue(roomId, out LiveStreamingRoom room))
        {
            room.RemoveClient(mainWebSocketService.GetClient(clientId));
        }
        else
        {
            Logger.LogError("Room not found");
        }
    }

    public override void OnRequestRoomBroadcast(string roomId, string message)
    {
        if (Rooms.TryGetValue(roomId, out LiveStreamingRoom room))
        {
            room.BroadcastToRoom(message);
        }
        else
        {
            Logger.LogError("Room not found");
        }
    }

    public override void OnRequestPasswordChange(string roomId, string clientId, string newPassword)
    {
        if (Rooms.TryGetValue(roomId, out LiveStreamingRoom room))
        {
            if (room.CheckAdmin(clientId))
            {
                room.ChangePassword(newPassword);
            }
            else
            {
                Logger.LogError("Client not admin");
            }
        }
        else
        {
            Logger.LogError("Room not found");
        }
    }

    public override void OnRequestRoomInfo(string roomId, string clientId)
    {
        if (Rooms.TryGetValue(roomId, out LiveStreamingRoom room))
        {
            mainWebSocketService.GetClient(clientId).Emit("RoomInfo", room.ToString());
        }
        else
        {
            Logger.LogError("Room not found");
        }
    }

    /// <summary>
    /// Receive data from openface and
    /// </summary>
    public void OnActionUnitData(string roomId, string clientId, string data)
    {
        if (Rooms.TryGetValue(roomId, out LiveStreamingRoom room))
        {
            IncomingData? incomingData = JsonConvert.DeserializeObject<IncomingData>(data);
            
            
            if (incomingData == null)
            {
                Logger.LogError("Data is null");
                return;
            }

            AU2BlendShapes au2BlendShapes = new AU2BlendShapes(incomingData);
            room.BroadcastToRoom(new WebsocketMessage(EnumEvents.LiveStreamingData.Name, au2BlendShapes.ToString()).ToJson());
        }
        else
        {
            Logger.LogError("Room not found");
        }
    }
    
    public void OnAudioData(string roomId, string clientId,string audioData)
    {
        if (Rooms.TryGetValue(roomId, out LiveStreamingRoom room))
        {
            
            room.BroadcastToRoom(audioData);
        }
        else
        {
            Logger.LogError("Room not found");
        }
    }

    /// <summary>
    /// send to openface the room id which the client is in, should be done once after room is created
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="roomId"></param>
    public void EmitRoomId(string clientId, string roomId)
    {
        mainWebSocketService.GetClient(clientId).Emit("RoomId", roomId);
    }
}