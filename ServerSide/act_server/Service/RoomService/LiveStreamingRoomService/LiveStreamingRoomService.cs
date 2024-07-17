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
        _mainWebSocketService.RegisterWebsocketService<ConnectionBehavior>("/mediapipe/blendshapedata/" + room.RoomId);
        
        logger.LogInformation("Room created with id: " + room.RoomId);
        room.InitRoom(roomCreationData.RoomOwner ?? "", roomCreationData.RoomName, roomCreationData.Password);
        
        try
        {   
            Rooms.Add(room.RoomId.ToString(), room);
            room.AddClient(mainWebSocketService.GetClient(roomCreationData.RoomOwner!));
            RoomInfo roomInfo = new RoomInfo(room.RoomName, room.RoomOwner, room.RoomId.ToString(), room.HasPassword(), room.Clients.Count);
            mainWebSocketService.GetClient(roomCreationData.RoomOwner!).Emit(EnumEvents.LiveStreamingRoomCreated.Name, roomInfo.ToJson() );
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
            if (room.VerifyPassword(password??""))
            {
                room.AddClient(mainWebSocketService.GetClient(clientId));
                mainWebSocketService.GetClient(clientId).Emit("RoomJoined", room.RoomId.ToString());
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
            mainWebSocketService.GetClient(clientId).Emit("RoomLeft", room.RoomId.ToString());
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

    public void OnRequestLiveStreamRoomInfos(string clientId)
    {
        List<object> roomInfos = new List<object>();
        if (Rooms.Values.Count == 0)
        {
            mainWebSocketService.GetClient(clientId).Emit(EnumEvents.LiveStreamingRoomsInfos.Name, roomInfos);
            return;
        }

        foreach (LiveStreamingRoom room in Rooms.Values)
        {
            roomInfos.Add(new RoomInfo(room.RoomName, room.RoomOwner, room.RoomId.ToString(), room.HasPassword(),
                room.Clients.Count).ToJson());
        }

        mainWebSocketService.GetClient(clientId).Emit(EnumEvents.LiveStreamingRoomsInfos.Name, roomInfos);
    }

    public struct poseData
    {
        public double pose_Tx { get; set; }
        public double pose_Ty { get; set; }
        public double pose_Tz { get; set; }
        public double pose_Rx { get; set; }
        public double pose_Ry { get; set; }
        public double pose_Rz { get; set; }
        public double frame;
        public double timestamp;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(new { pose_Tx, pose_Ty, pose_Tz, pose_Rx, pose_Ry, pose_Rz, frame });
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
            Pose pose = incomingData.pose;
            poseData poseData = new poseData();
            poseData.pose_Tx = pose.pose_Tx;
            poseData.pose_Ty = pose.pose_Ty;
            poseData.pose_Tz = pose.pose_Tz;
            poseData.pose_Rx = pose.pose_Rx;
            poseData.pose_Ry = pose.pose_Ry;
            poseData.pose_Rz = pose.pose_Rz;
            poseData.frame = incomingData.frame;
            poseData.timestamp = incomingData.timestamp;
            
            
            room.BroadcastToRoom(new WebsocketMessage(EnumEvents.LiveStreamingData.Name, au2BlendShapes.ToString()).ToJson());
            room.BroadcastToRoom(new WebsocketMessage(EnumEvents.LiveStreamingAvatarHeadPose.Name, poseData.ToString() ).ToJson());
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
    
    public void OnMediaPipeBlendshapeData(string roomId, string clientId, List<BlendShapeData> data)
    {
        if (Rooms.TryGetValue(roomId, out LiveStreamingRoom room))
        {
            
            room.BroadcastToRoom(new WebsocketMessage(EnumEvents.LiveStreamingMediaPipeBlendshape.Name, data).ToJson());
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