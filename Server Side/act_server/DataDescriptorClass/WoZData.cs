using act_server.Service.RoomService.WoZRoomService;

namespace act_server.DataDescriptorClass;

public record WoZAvatarHeadMoveData : AbstractRoomEventData<AvatarHeadMoveData>
{
    public WoZAvatarHeadMoveData(string roomId, AvatarHeadMoveData headMoveData)
    {
        this.roomId = roomId;
        this.Data = headMoveData;
    }

    protected sealed override AvatarHeadMoveData Data { get; set; }

    public string RoomId => roomId;
    public AvatarHeadMoveData HeadMoveData => Data;
}

public record WoZAvatarBlendshapeMoveData : AbstractRoomEventData<AvatarBlendshapeMoveData>
{
    public WoZAvatarBlendshapeMoveData(string roomId, AvatarBlendshapeMoveData blendshapeMoveData)
    {
        this.roomId = roomId;
        this.Data = blendshapeMoveData;
    }

    protected sealed override AvatarBlendshapeMoveData Data { get; set; }

    public string RoomId => roomId;
    public AvatarBlendshapeMoveData BlendshapeMoveData => Data;
}

public record WoZAvatarBlendshapeTransitionData : AbstractRoomEventData<AvatarBlendshapeTransitionData>
{
    public WoZAvatarBlendshapeTransitionData(string roomId, AvatarBlendshapeTransitionData blendshapeTransitionData)
    {
        this.roomId = roomId;
        this.Data = blendshapeTransitionData;
    }

    protected sealed override AvatarBlendshapeTransitionData Data { get; set; }

    public string RoomId => roomId;
    public AvatarBlendshapeTransitionData BlendshapeTransitionData => Data;
}

public record WoZAvatarPoseTransitionData : AbstractRoomEventData<AvatarPoseTransitionData>
{
    public WoZAvatarPoseTransitionData(string roomId, AvatarPoseTransitionData poseTransitionData)
    {
        this.roomId = roomId;
        this.Data = poseTransitionData;
    }

    protected sealed override AvatarPoseTransitionData Data { get; set; }

    public string RoomId => roomId;
    public AvatarPoseTransitionData PoseTransitionData => Data;
}