using System;
using System.Collections.Generic;
using act_server.Enum;
using act_server.Room;
using act_server.Service.RoomService;
using act_server.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace act_server.Controller.LiveStreamingRoomController;

public sealed class LiveStreamingRoomController
    : AbstractRoomController<LiveStreamingRoomService, LiveStreamingRoom>
{
    protected override EventManagerService? PEventManagerService { get; set; }
    protected override LiveStreamingRoomService? PRoomService { get; set; }


    public LiveStreamingRoomController(IServiceProvider serviceProvider)
    {
        PEventManagerService = serviceProvider.GetRequiredService<EventManagerService>();
        PRoomService = serviceProvider.GetRequiredService<LiveStreamingRoomService>();
        PLogger = serviceProvider.GetRequiredService<ILogger<LiveStreamingRoomController>>();
        RegisterEvents();
    }


    protected override ILogger<AbstractRoomController<LiveStreamingRoomService, LiveStreamingRoom>>? PLogger
    {
        get;
        set;
    }

    protected override void RegisterEvents()
    {
        if (PLogger != null) PLogger.LogInformation("Registering events");
        if (PEventManagerService == null) throw new Exception("EventManagerService is null");
        PEventManagerService.On(EnumEvents.RequestLiveStreamingRoom.Name, OnRequestRoomCreate);
        PEventManagerService.On(EnumEvents.RequestLiveStreamingRoomJoin.Name, OnRequestRoomJoin);
        PEventManagerService.On(EnumEvents.RequestPasswordChange.Name, OnRequestPasswordChange);
        PEventManagerService.On(EnumEvents.RequestRoomInfo.Name, OnRequestRoomInfo);
        PEventManagerService.On(EnumEvents.RequestLiveStreamingRooms.Name, OnRequestLiveStreamRoomInfos);

        PEventManagerService.On(EnumEvents.EmitActionUnit.Name, OnActionUnitData);
        PEventManagerService.On(EnumEvents.EmitAudioData.Name, OnAudioData);
        PEventManagerService.On(EnumEvents.MediapipeBlendshape.Name, OnMediaPipeBlendshapeData);
        
    }

    /// <summary>
    /// Process the request to create a room
    /// </summary>
    /// <param name="data">data receive from unity</param>
    /// <param name="clientId">cientID is the sesssion Id of the user</param>
    protected override void OnRequestRoomCreate(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;
        try
        {
            RoomCreationData roomCreationData = JsonConvert.DeserializeObject<RoomCreationData>(data);


            var roomCreationDataFormat =
                new RoomCreationData(roomCreationData.RoomName, clientId, roomCreationData.Password);
            PRoomService.OnRequestRoomCreate(roomCreationDataFormat);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing room creation request.");
        }
    }

    public struct RequestRoomJoinData
    {
        public string RoomId;
        public string Password;
    }
    /// <summary>
    ///  Process the request to join a room 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="clientId"></param>
    protected override void OnRequestRoomJoin(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;

        try
        {
            RequestRoomJoinData roomJoinData = JsonConvert.DeserializeObject<RequestRoomJoinData>(data);
            if (roomJoinData.Equals(null))
            {
                PLogger.LogError("Room join data is null.");
                return;
            }

            PRoomService.OnRequestRoomJoin(roomJoinData.RoomId, clientId, roomJoinData.Password);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing room join request.");
        }
    }

    /// <summary>
    ///  Process the request to leave a room
    /// </summary>
    /// <param name="data"></param>
    /// <param name="clientId"></param>
    protected override void OnRequestRoomLeave(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;

        try
        {
            dynamic roomLeaveData = JsonConvert.DeserializeObject(data);
            if (roomLeaveData == null)
            {
                PLogger.LogError("Room leave data is null.");
                return;
            }

            PRoomService.OnRequestRoomLeave(roomLeaveData.roomId, clientId);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing room leave request.");
        }
    }

    protected override void OnRequestRoomBroadcast(string data, string s)
    {
        if (PLogger == null || PRoomService == null) return;

        try
        {
            dynamic roomBroadcastData = JsonConvert.DeserializeObject(data);
            if (roomBroadcastData == null)
            {
                PLogger.LogError("Room broadcast data is null.");
                return;
            }

            PRoomService.OnRequestRoomBroadcast(roomBroadcastData.roomId, roomBroadcastData.message);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing room broadcast request.");
        }
    }

    protected override void OnRequestPasswordChange(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;

        try
        {
            dynamic passwordChangeData = JsonConvert.DeserializeObject(data);
            if (passwordChangeData == null)
            {
                PLogger.LogError("Password change data is null.");
                return;
            }

            PRoomService.OnRequestPasswordChange(passwordChangeData.roomId, clientId,
                passwordChangeData.newPassword);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing password change request.");
        }
    }

    protected override void OnRequestRoomInfo(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;

        try
        {
            dynamic roomInfoData = JsonConvert.DeserializeObject(data);
            if (roomInfoData == null)
            {
                PLogger.LogError("Room info data is null.");
                return;
            }

            PRoomService.OnRequestRoomInfo(roomInfoData.roomId, clientId);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing room info request.");
        }
    }

    private void OnRequestLiveStreamRoomInfos(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;

        try
        {
            PRoomService.OnRequestLiveStreamRoomInfos(clientId);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing live stream room infos request.");
        }
        
    }

    private void OnActionUnitData(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;
        try
        {
            IncomingData? actionUnitData = JsonConvert.DeserializeObject<IncomingData>(data);
            if (actionUnitData == null)
            {
                PLogger.LogError("Action unit data is null.");
                return;
            }

            PRoomService.OnActionUnitData(actionUnitData.roomId, clientId, data);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing action unit request.");
        }
    }

    private void OnAudioData(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;

        try
        {
            LiveStreamingRoom.AudioData audioData = JsonConvert.DeserializeObject<LiveStreamingRoom.AudioData>(data);
            if (audioData.Equals(null))
            {
                PLogger.LogError("Audio data is null.");
                return;
            }

            PRoomService.OnAudioData(audioData.roomId, clientId, audioData);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing audio data request.");
        }
    }
    
    public class MediaPipeBlendshapeData
    {
        public string RoomId;
        public List<BlendShapeData> BlendShapeList ;
    }
    
    private void OnMediaPipeBlendshapeData(string data, string clientId)
    {
        if (PLogger == null || PRoomService == null) return;

        try
        {
            PLogger.LogInformation("Received mediapipe blendshape data" + data);
            MediaPipeBlendshapeData blendshapeData = JsonConvert.DeserializeObject<MediaPipeBlendshapeData>(data);
            PLogger.LogInformation("Received mediapipe blendshape data" + blendshapeData.BlendShapeList);
            
            if (blendshapeData == null)
            {
                PLogger.LogError("Blendshape data is null.");
                return;
            }

            PRoomService.OnMediaPipeBlendshapeData(blendshapeData.RoomId, clientId, blendshapeData.BlendShapeList);
        }
        catch (Exception ex)
        {
            PLogger.LogError(ex, "Error processing blendshape data request.");
        }
    }
}