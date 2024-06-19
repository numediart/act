using act_server.Enum;

namespace act_server.DataDescriptorClass;

public abstract record AbstractRoomEventData<TData> where TData : struct
{
    protected string roomId;
    protected abstract TData Data { get; set; }
}