using System;
using act_server.DataDescriptorClass;
using act_server.Enum;
using act_server.Room;
using act_server.Service.RoomService.WoZRoomService;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace act_server.Controller.WoZRoomController
{
    /// <summary>
    /// Class to control the WoZRoom, and process WoZRoom events
    /// </summary>
    public class WoZRoomController : AbstractRoomController<WoZRoomService, WoZRoom>
    {
        protected sealed override EventManagerService? PEventManagerService { get; set; }
        protected sealed override WoZRoomService? PRoomService { get; set; }
        protected sealed override ILogger<AbstractRoomController<WoZRoomService, WoZRoom>>? PLogger { get; set; }

        public WoZRoomController(IServiceProvider serviceProvider)
        {
            PEventManagerService = serviceProvider.GetRequiredService<EventManagerService>();
            PRoomService = serviceProvider.GetRequiredService<WoZRoomService>();
            PLogger = serviceProvider.GetRequiredService<ILogger<WoZRoomController>>();

            RegisterEvents();
        }

        #region Register WoZRoom Events

        /// <summary>
        /// Register events to the event manager service
        /// </summary>
        protected sealed override void RegisterEvents()
        {
            if (PEventManagerService == null) return;
            PEventManagerService.On(EnumEvents.RequestWoZRoomCreation.Name, OnRequestRoomCreate);
            PEventManagerService.On(EnumEvents.RequestWoZRoomJoin.Name, OnRequestRoomJoin);
            PEventManagerService.On(EnumEvents.RequestWoZRoomLeave.Name, OnRequestRoomLeave);
            PEventManagerService.On(EnumEvents.RequestWoZRoomBroadcast.Name, OnRequestRoomBroadcast);
            PEventManagerService.On(EnumEvents.RequestPasswordChange.Name, OnRequestPasswordChange);
            PEventManagerService.On(EnumEvents.RequestRoomInfo.Name, OnRequestRoomInfo);
            PEventManagerService.On(EnumEvents.RequestAvatarHeadMove.Name, OnRequestAvatarHeadMove);
            PEventManagerService.On(EnumEvents.RequestAvatarBlendshapeMove.Name, OnRequestAvatarBlendshapeMove);
            PEventManagerService.On(EnumEvents.RequestAvatarBlendshapeTransition.Name,
                OnRequestAvatarBlendshapeTransition);
            PEventManagerService.On(EnumEvents.RequestAvatarPoseTransition.Name, OnRequestAvatarPoseTransition);
        }

        #endregion

        /// <summary>
        /// Process the request to create a room
        /// </summary>
        /// <param name="data"></param>
        /// <param name="clientId"></param>
        protected override void OnRequestRoomCreate(string data, string clientId)
        {
            if (PLogger == null || PRoomService == null) return;
            try
            {
                RoomCreationData roomCreationData = JsonConvert.DeserializeObject<RoomCreationData>(data);
                if (roomCreationData.Equals(null))
                {
                    PLogger.LogError("Room creation data is null.");
                    return;
                }

                var roomCreationDataFormat =
                    new RoomCreationData(roomCreationData.RoomName, clientId, roomCreationData.Password);
                PRoomService.OnRequestRoomCreate(roomCreationDataFormat);
            }
            catch (Exception ex)
            {
                PLogger.LogError(ex, "Error processing room creation request.");
            }
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
                dynamic roomJoinData = JsonConvert.DeserializeObject(data);
                if (roomJoinData == null)
                {
                    PLogger.LogError("Room join data is null.");
                    return;
                }

                PRoomService.OnRequestRoomJoin(roomJoinData.roomId, clientId, roomJoinData.password);
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

        private void OnRequestAvatarHeadMove(string data, string clientId)
        {
            if (PLogger == null || PRoomService == null) return;

            try
            {
                var avatarHeadMoveData = JsonConvert.DeserializeObject<WoZAvatarHeadMoveData>(data);
                if (avatarHeadMoveData == null)
                {
                    PLogger.LogError("Avatar head move data is null.");
                    return;
                }

                PRoomService.OnRequestAvatarHeadMove(avatarHeadMoveData.RoomId, clientId,
                    avatarHeadMoveData.HeadMoveData);
            }
            catch (Exception ex)
            {
                PLogger.LogError(ex, "Error processing avatar head move request.");
            }
        }

        private void OnRequestAvatarBlendshapeMove(string data, string clientId)
        {
            if (PLogger == null || PRoomService == null) return;

            try
            {
                var avatarBlendshapeMoveData = JsonConvert.DeserializeObject<WoZAvatarBlendshapeMoveData>(data);
                if (avatarBlendshapeMoveData == null)
                {
                    PLogger.LogError("Avatar blendshape move data is null.");
                    return;
                }

                PRoomService.OnRequestAvatarBlendshapeMove(avatarBlendshapeMoveData.RoomId, clientId,
                    avatarBlendshapeMoveData.BlendshapeMoveData);
            }
            catch (Exception ex)
            {
                PLogger.LogError(ex, "Error processing avatar blendshape move request.");
            }
        }

        private void OnRequestAvatarBlendshapeTransition(string data, string clientId)
        {
            if (PLogger == null || PRoomService == null) return;

            try
            {
                var avatarBlendshapeTransitionData =
                    JsonConvert.DeserializeObject<WoZAvatarBlendshapeTransitionData>(data);
                if (avatarBlendshapeTransitionData == null)
                {
                    PLogger.LogError("Avatar blendshape transition data is null.");
                    return;
                }

                PRoomService.OnRequestAvatarBlendshapeTransition(avatarBlendshapeTransitionData.RoomId, clientId,
                    avatarBlendshapeTransitionData.BlendshapeTransitionData);
            }
            catch (Exception ex)
            {
                PLogger.LogError(ex, "Error processing avatar blendshape transition request.");
            }
        }

        private void OnRequestAvatarPoseTransition(string data, string clientId)
        {
            if (PLogger == null || PRoomService == null) return;

            try
            {
                var avatarPoseTransitionData = JsonConvert.DeserializeObject<WoZAvatarPoseTransitionData>(data);
                if (avatarPoseTransitionData == null)
                {
                    PLogger.LogError("Avatar pose transition data is null.");
                    return;
                }

                PRoomService.OnRequestAvatarPoseTransition(avatarPoseTransitionData.RoomId, clientId,
                    avatarPoseTransitionData.PoseTransitionData);
            }
            catch (Exception ex)
            {
                PLogger.LogError(ex, "Error processing avatar pose transition request.");
            }
        }
    }
}