using _Scripts._Version_1._0.Managers.Network.WebSocket.Enum;

namespace _Scripts._Version_1._0.Managers.Network.WebSocket.Enum
{

    public class EnumEvents : Enumeration<EnumEvents>
    {
        private static int _nextValue = 1;

        private static int GetNextValue()
            // allow to have dynamic value for enum value if need to add more enum or change the order
            // as using in most cases the name of the enum instead of the value so the value is not that important
        {
            return _nextValue++;
        }

        #region Websocket Events

        #region Client Connection Events

        // create Client events
        public static readonly EnumEvents
            ClientConnected = new(GetNextValue(), "ClientConnected"); // get the next value for the enum should be 1

        public static readonly EnumEvents
            ClientDisconnected =
                new(GetNextValue(), "ClientDisconnected"); // get the next value for the enum should be 2 and so on

        #endregion

        #region Face Detection Events Software (here openface)

        public static readonly EnumEvents EmitActionUnit = new EnumEvents(GetNextValue(), "EmitActionUnit");
        public static readonly EnumEvents EmitAudioData = new EnumEvents(GetNextValue(), "EmitAudioData");
        public static readonly EnumEvents OnRoomId = new EnumEvents(GetNextValue(), "OnRoomId");

        #endregion

        #region Room Events

        // create Room events
        public static readonly EnumEvents RoomCreated = new(GetNextValue(), "RoomCreated");
        public static readonly EnumEvents RoomDeleted = new(GetNextValue(), "RoomDeleted");
        public static readonly EnumEvents RequestRoomCreation = new(GetNextValue(), "RequestRoomCreation");
        public static readonly EnumEvents RequestRoomDeletion = new(GetNextValue(), "RequestRoomDeletion");
        public static readonly EnumEvents RequestRoomJoin = new(GetNextValue(), "RequestRoomJoin");

        #endregion

        #region Live Stream Events

        // create LiveStreamingRoom events
        public static readonly EnumEvents RequestLiveStreamingRoom = new(GetNextValue(), "RequestLiveStreamingRoom");
        public static readonly EnumEvents LiveStreamingRoomCreated = new(GetNextValue(), "LiveStreamingRoomCreated");
        public static readonly EnumEvents LiveStreamingData = new(GetNextValue(), "LiveStreamingData");

        public static readonly EnumEvents LiveStreamingAvatarHeadPose =
            new(GetNextValue(), "LiveStreamingAvatarHeadPose");

        public static readonly EnumEvents
            LiveStreamingAvatarEyeGaze = new(GetNextValue(), "LiveStreamingAvatarEyeGaze");

        public static readonly EnumEvents LiveStreamingAudioData = new(GetNextValue(), "LiveStreamingAudioData");

        #endregion

        #region WoZRoom Events

        // create WoZRoom events
        public static readonly EnumEvents RequestWoZRoomCreation = new(GetNextValue(), "RequestWoZRoomCreation");
        public static readonly EnumEvents RequestWoZRoomDeletion = new(GetNextValue(), "RequestWoZRoomDeletion");
        public static readonly EnumEvents RequestWoZRoomJoin = new(GetNextValue(), "RequestWoZRoomJoin");
        public static readonly EnumEvents RequestWoZRoomLeave = new(GetNextValue(), "RequestWoZRoomLeave");
        public static readonly EnumEvents RequestWoZRoomBroadcast = new(GetNextValue(), "RequestWoZRoomBroadcast");
        public static readonly EnumEvents RequestPasswordChange = new(GetNextValue(), "RequestPasswordChange");
        public static readonly EnumEvents RequestRoomInfo = new(GetNextValue(), "RequestRoomInfo");
        public static readonly EnumEvents RequestAvatarHeadMove = new(GetNextValue(), "RequestAvatarHeadMove");

        public static readonly EnumEvents RequestAvatarBlendshapeMove =
            new(GetNextValue(), "RequestAvatarBlendshapeMove");

        public static readonly EnumEvents RequestAvatarBlendshapeTransition =
            new(GetNextValue(), "RequestAvatarBlendshapeTransition");

        public static readonly EnumEvents RequestAvatarPoseTransition =
            new(GetNextValue(), "RequestAvatarPoseTransition");

        public static readonly EnumEvents AvatarHeadMove = new(GetNextValue(), "AvatarHeadMove");
        public static readonly EnumEvents AvatarBlendshapeMove = new(GetNextValue(), "AvatarBlendshapeMove");
        public static readonly EnumEvents AvatarBlendshapeTransition = new(GetNextValue(), "AvatarBlendshapeTransition");
        public static readonly EnumEvents AvatarPoseTransition = new(GetNextValue(), "AvatarPoseTransition");

        public static readonly EnumEvents WoZRoomInfo = new(GetNextValue(), "WoZRoomInfo");
        public static readonly EnumEvents WoZRoomCreated = new(GetNextValue(), "WoZRoomCreated");
        public static readonly EnumEvents WoZRoomDeleted = new(GetNextValue(), "WoZRoomDeleted");
        public static readonly EnumEvents WoZRoomJoined = new(GetNextValue(), "WoZRoomJoined");
        public static readonly EnumEvents WoZRoomLeft = new(GetNextValue(), "WoZRoomLeft");

        #endregion

        #endregion

        private EnumEvents(int value, string name) : base(value, name)
        {
        }
    }
}