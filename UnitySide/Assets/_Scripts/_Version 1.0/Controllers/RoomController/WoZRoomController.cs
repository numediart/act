using System.Collections.Generic;
using _Scripts._Version_1._0.Managers.Network.WebSocket.Enum;
using _Scripts._Version_1._0.Services.RoomServices.WoZRoomService;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts._Version_1._0.Controllers.RoomController
{
    public class WoZRoomController : AbstractRoomController<WoZRoomController>
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
            Managers.Network.WebSocket.EventManager.Instance.On(EnumEvents.AvatarBlendshapeMove.Name,
                OnAvatarBlendshapeMove);
            Managers.Network.WebSocket.EventManager.Instance.On(EnumEvents.AvatarBlendshapeTransition.Name,
                OnBlendshapesTransition);
            Managers.Network.WebSocket.EventManager.Instance.On(EnumEvents.AvatarPoseTransition.Name,
                OnAvatarPoseTransition);
        }

        protected override void UnregisterEvents()
        {
            Managers.Network.WebSocket.EventManager.Instance.Off(EnumEvents.AvatarHeadMove.Name, OnAvatarHeadMove);
            Managers.Network.WebSocket.EventManager.Instance.Off(EnumEvents.AvatarBlendshapeMove.Name,
                OnAvatarBlendshapeMove);
            Managers.Network.WebSocket.EventManager.Instance.Off(EnumEvents.AvatarBlendshapeTransition.Name,
                OnBlendshapesTransition);
            Managers.Network.WebSocket.EventManager.Instance.Off(EnumEvents.AvatarPoseTransition.Name,
                OnAvatarPoseTransition);
        }

        protected override void OnRoomCreated(string data)
        {
            throw new System.NotImplementedException();
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
            try
            {
                AvatarHeadMoveData avatarHeadMoveData = JsonConvert.DeserializeObject<AvatarHeadMoveData>(data);
                _woZRoomService.OnAvatarHeadMove(avatarHeadMoveData);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }

        private void OnAvatarBlendshapeMove(string data)
        {
            try
            {
                Dictionary<string, double> avatarBlendshapeData =
                    JsonConvert.DeserializeObject<Dictionary<string, double>>(data);
                _woZRoomService.OnAvatarBlendshapeMove(avatarBlendshapeData);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }

        public struct BlendshapeTransitionData
        {
            public Dictionary<string, double> blendshapeDict;
            public float duration;
        }

        private void OnBlendshapesTransition(string data)
        {
            try
            {
                BlendshapeTransitionData avatarBlendshapeData =
                    JsonConvert.DeserializeObject<BlendshapeTransitionData>(data);
                Debug.Log(avatarBlendshapeData.blendshapeDict.Keys);
                _woZRoomService.OnBlendshapesTransition(avatarBlendshapeData.blendshapeDict,
                    avatarBlendshapeData.duration);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }

        struct AvatarPoseData
        {
            public Vector3 Position;
            public float Duration;

            public AvatarPoseData(float x, float y, float z, float duration)
            {
                Position = new Vector3(x, y, z);
                this.Duration = duration;
            }
        }

        private void OnAvatarPoseTransition(string data)
        {
            try
            {
                AvatarPoseData avatarPoseData = JsonConvert.DeserializeObject<AvatarPoseData>(data);
                Debug.Log(avatarPoseData.Position);
                _woZRoomService.OnAvatarPoseTransition(avatarPoseData.Position, avatarPoseData.Duration);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}