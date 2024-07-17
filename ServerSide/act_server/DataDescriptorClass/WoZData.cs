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
    public WoZAvatarBlendshapeMoveData(string roomId, Dictionary<string, double> blendshapeDict)
    {
        this.roomId = roomId;
        this.Data = new AvatarBlendshapeMoveData { BlendshapeDict = blendshapeDict };
    }

    protected sealed override AvatarBlendshapeMoveData Data { get; set; }

    public string RoomId => roomId;
    public AvatarBlendshapeMoveData BlendshapeMoveData => Data;
}

public record WoZAvatarBlendshapeTransitionData {
    [JsonProperty("roomId")]
    public string RoomId { get; set; }

    [JsonProperty("blendshapeTransitionData")]
    public BlendshapeTransitionData BlendshapeTransitionData { get; set; }

    [JsonProperty("Duration")]
    public float Duration { get; set; }
}

public class BlendshapeTransitionData {
    [JsonProperty("BlendshapeDict")]
    public Dictionary<string, double> BlendshapeDict { get; set; }

    // If Duration is part of this inner object based on your JSON structure
    // Adjust the type if necessary, e.g., if it should be a string or a different type
    [JsonProperty("Duration")]
    public float Duration { get; set; }
}

public record WoZAvatarPoseTransitionData : AbstractRoomEventData<AvatarPoseTransitionData>
{
    public WoZAvatarPoseTransitionData(string roomId, double x, double y, double z, string duration)
    {
        this.roomId = roomId;
        this.Data = new AvatarPoseTransitionData { x = x, y = (float)y, z = (float)z, Duration = duration };
    }

    protected sealed override AvatarPoseTransitionData Data { get; set; }

    public string RoomId => roomId;
    public AvatarPoseTransitionData PoseTransitionData => new AvatarPoseTransitionData { x = Data.x, y = Data.y, z = Data.z, Duration = Data.Duration };
}