using System.Collections.Generic;
using _Scripts._Version_1._0.Managers.Network.WebSocket;
using _Scripts._Version_1._0.Managers.Network.WebSocket.Enum;
using _Scripts._Version_1._0.Services.RoomServices.LiveStreamingRoomService;
using Newtonsoft.Json;
using UnityEngine;
using Pose = _Scripts._Version_1._0.Utils.Pose;

namespace _Scripts._Version_1._0.Controllers.RoomController
{
    public class LiveStreamingRoomController : AbstractRoomController<LiveStreamingRoomController>
    {
        protected virtual EventManager EventManager { get; set; } = EventManager.Instance;

        private IncomingLiveStreamData _incomingLiveStreamData;
        private IncomingLiveStreamData _previousIncomingLiveStreamData;

        private LiveStreamingRoomService _liveStreamingRoomService;

        public LiveStreamingRoomController(LiveStreamingRoomService liveStreamingRoomService)
        {
            _liveStreamingRoomService = liveStreamingRoomService;
            RegisterEvents();
        }

        protected sealed override void RegisterEvents()
        {
            EventManager.On(EnumEvents.LiveStreamingRoomCreated.Name, OnRoomCreated);
            EventManager.On(EnumEvents.LiveStreamingData.Name, OnActionUnitsReceived);
            EventManager.On(EnumEvents.LiveStreamingAvatarHeadPose.Name, OnHeadPoseReceived);
            EventManager.On(EnumEvents.LiveStreamingMediaPipeBlendshape.Name, OnMediaPipeBlendshapeData);
            
        }

        protected override void UnregisterEvents()
        {
            EventManager.Off(EnumEvents.LiveStreamingRoomCreated.Name, OnRoomCreated);
            EventManager.Off(EnumEvents.LiveStreamingData.Name, OnActionUnitsReceived);
            EventManager.Off(EnumEvents.LiveStreamingAvatarHeadPose.Name, OnHeadPoseReceived);
            EventManager.Off(EnumEvents.LiveStreamingMediaPipeBlendshape.Name, OnMediaPipeBlendshapeData);
        }

        protected override void OnRoomCreated(string data)
        {
            Debug.Log("Room Created , Data : " + data);
            
        }

        protected override void OnRoomJoined(string data)
        {
            Debug.Log("Room Joined , Data : " + data);
        }

        protected override void OnRoomLeft(string data)
        {
            Debug.Log("Room Left , Data : " + data);
        }

        private void OnActionUnitsReceived(string data)
        {
            Debug.Log("Action Units Received , Data : " + data);
            _incomingLiveStreamData = JsonConvert.DeserializeObject<IncomingLiveStreamData>(data);
            _liveStreamingRoomService.OnActionUnitsReceived(_incomingLiveStreamData);
        }

        private void OnHeadPoseReceived(string data)
        {
            var pose = JsonConvert.DeserializeObject<Pose>(data);
            _liveStreamingRoomService.OnHeadPoseReceived(pose);
        }
        
        
        private void OnMediaPipeBlendshapeData(string data)
        {
            var blendShapeData = JsonConvert.DeserializeObject<List<BlendShapeList>>(data);
            
            _liveStreamingRoomService.OnMediaPipeBlendshapeData(blendShapeData);
        }
    }
}