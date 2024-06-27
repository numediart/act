using System.Collections.Generic;
using act_server.Service.RoomService.WoZRoomService;
using Newtonsoft.Json;

namespace act_server.DataDescriptorClass;

public record WoZAvatarHeadMoveData : AbstractRoomEventData<AvatarHeadMoveData>
{
    public string roomId;
    public  AvatarHeadMoveData HeadData { get; set; }
    public WoZAvatarHeadMoveData(string roomId, double x, double y, double z)
    {
        this.roomId = roomId;
        this.Data = new AvatarHeadMoveData { x = x, y = y, z = z };
        HeadData = Data;
    }

    protected sealed override AvatarHeadMoveData Data { get; set; }

    public string RoomId => roomId;
    public AvatarHeadMoveData HeadMoveData => Data;
}

public record WoZAvatarBlendshapeMoveData : AbstractRoomEventData<AvatarBlendshapeMoveData>
{
    public WoZAvatarBlendshapeMoveData(string roomId, string blendshapeDict, string value)
    {
        this.roomId = roomId;
        this.Data = new AvatarBlendshapeMoveData { BlendshapeDict = blendshapeDict, Value = value };
    }

    protected sealed override AvatarBlendshapeMoveData Data { get; set; }

    public string RoomId => roomId;
    public AvatarBlendshapeMoveData BlendshapeMoveData => Data;
}

public record WoZAvatarBlendshapeTransitionData {
    [JsonProperty("roomId")]
    public string RoomId { get; set; }

    [JsonProperty("blendshapeTransitionData")]
    public Dictionary<string, double> BlendshapeTransitionData { get; set; }

    [JsonProperty("Duration")]
    public float Duration { get; set; }
}

public record WoZAvatarPoseTransitionData : AbstractRoomEventData<AvatarPoseTransitionData>
{
    public WoZAvatarPoseTransitionData(string roomId, double x, double y, double z, string duration)
    {
        this.roomId = roomId;
        this.Data = new AvatarPoseTransitionData { x = (float)x, y = (float)y, z = (float)z, Duration = duration };
    }

    protected sealed override AvatarPoseTransitionData Data { get; set; }

    public string RoomId => roomId;
    public AvatarPoseTransitionData PoseTransitionData => Data;
}