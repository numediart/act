using System.Collections.Generic;
using _Scripts._Version_1._0.Managers.Network.WebSocket.Enum;
using _Scripts._Version_1._0.Services.RoomServices.WoZRoomService;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts._Version_1._0.Controllers.RoomController
{
    public class WoZRoomController:AbstractRoomController<WoZRoomController>
    {
        
        WoZRoomService _woZRoomService;
        public WoZRoomController(WoZRoomService woZRoomService)
        {
            _woZRoomService = woZRoomService;
            RegisterEvents();
        }
        
        protected sealed override void RegisterEvents()
        {
            Managers.Network.WebSocket.EventManager.Instance.On(EnumEvents.AvatarHeadMove.Name, OnAvatarHeadMove);
            Managers.Network.WebSocket.EventManager.Instance.On(EnumEvents.AvatarBlendshapeMove.Name, OnAvatarBlendshapeMove);
            Managers.Network.WebSocket.EventManager.Instance.On(EnumEvents.AvatarBlendshapeTransition.Name, OnBlendshapesTransition);
            Managers.Network.WebSocket.EventManager.Instance.On(EnumEvents.AvatarPoseTransition.Name, OnAvatarPoseTransition);
            
        }

        protected override void UnregisterEvents()
        {
            Managers.Network.WebSocket.EventManager.Instance.Off(EnumEvents.AvatarHeadMove.Name, OnAvatarHeadMove);
            Managers.Network.WebSocket.EventManager.Instance.Off(EnumEvents.AvatarBlendshapeMove.Name, OnAvatarBlendshapeMove);
            Managers.Network.WebSocket.EventManager.Instance.Off(EnumEvents.AvatarBlendshapeTransition.Name, OnBlendshapesTransition);
            Managers.Network.WebSocket.EventManager.Instance.Off(EnumEvents.AvatarPoseTransition.Name, OnAvatarPoseTransition);
        }
        
        protected override void OnRoomCreated(string data)
        {
            _woZRoomService.OnRoomCreated(data);
        }

        protected override void OnRoomJoined(string data)
        {
            _woZRoomService.OnRoomJoined(data);
        }

        protected override void OnRoomLeft(string data)
        {
            throw new System.NotImplementedException();
        }
        
        private void OnAvatarHeadMove(string data)
        {
            AvatarHeadMoveData avatarHeadMoveData = JsonConvert.DeserializeObject<AvatarHeadMoveData>(data);
            _woZRoomService.OnAvatarHeadMove(avatarHeadMoveData);
        }
        private void OnAvatarBlendshapeMove(string data)
        {
            Dictionary<string,double> avatarBlendshapeData = JsonConvert.DeserializeObject<Dictionary<string,double>>(data);
            _woZRoomService.OnAvatarBlendshapeMove(avatarBlendshapeData);
        }
        private void OnBlendshapesTransition(string data)
        {
            Dictionary<string,double> avatarBlendshapeData = JsonConvert.DeserializeObject<Dictionary<string,double>>(data);
            _woZRoomService.OnBlendshapesTransition(avatarBlendshapeData);
        }
        
        struct AvatarPoseData
        {
            public Vector3 position;
            public float duration;
        }
        private void OnAvatarPoseTransition(string data)
        {
            AvatarPoseData avatarPoseData = JsonConvert.DeserializeObject<AvatarPoseData>(data);
            _woZRoomService.OnAvatarPoseTransition(avatarPoseData.position, avatarPoseData.duration);
        }
    }
}