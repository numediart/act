namespace act_server.DataDescriptorClass;


public record struct RoomJoinData
{
    public readonly string? Password;

    public RoomJoinData(string? clientId, string? password)
    {
        
        Password = password;
    }
}

public record RequestRoomJoinData: AbstractRoomEventData<RoomJoinData>
{
    protected sealed override RoomJoinData Data { get; set; }
    
    public RequestRoomJoinData(string roomId, RoomJoinData roomJoinData)
    {
        this.roomId = roomId;
        this.Data = roomJoinData;
    }
}

public record struct RoomLeaveData
{
    public readonly string? ClientId;

    public RoomLeaveData(string? clientId)
    {
        ClientId = clientId;
    }
}

public record RequestRoomLeaveData: AbstractRoomEventData<RoomLeaveData>
{
    protected sealed override RoomLeaveData Data { get; set; }
    
    public RequestRoomLeaveData(string roomId, RoomLeaveData roomLeaveData)
    {
        this.roomId = roomId;
        this.Data = roomLeaveData;//
    }
}

public record struct RoomBroadcastData
{
    public readonly string? Message;

    public RoomBroadcastData(string? message)
    {
        Message = message;
    }
}

public record RequestRoomBroadcastData: AbstractRoomEventData<RoomBroadcastData>
{
    protected sealed override RoomBroadcastData Data { get; set; }
    
    public RequestRoomBroadcastData(string roomId, RoomBroadcastData roomBroadcastData)
    {
        this.roomId = roomId;
        this.Data = roomBroadcastData;
    }
}

public record struct RoomPasswordChangeData
{
    public readonly string? NewPassword;

    public RoomPasswordChangeData(string? newPassword)
    {
        NewPassword = newPassword;
    }
}

public record RequestRoomPasswordChangeData: AbstractRoomEventData<RoomPasswordChangeData>
{
    protected sealed override RoomPasswordChangeData Data { get; set; }
    
    public RequestRoomPasswordChangeData(string roomId, RoomPasswordChangeData roomPasswordChangeData)
    {
        this.roomId = roomId;
        this.Data = roomPasswordChangeData;
    }
}

public record struct RoomInfoData
{
    public readonly string? ClientId;

    public RoomInfoData(string? clientId)
    {
        ClientId = clientId;
    }
}

public record RequestRoomInfoData: AbstractRoomEventData<RoomInfoData>
{
    protected sealed override RoomInfoData Data { get; set; }
    
    public RequestRoomInfoData(string roomId, RoomInfoData roomInfoData)
    {
        this.roomId = roomId;
        this.Data = roomInfoData;
    }
}