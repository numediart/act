using System;

namespace act_server.Interface;


public interface IWebsocketPayload
{
    public string EventName { get; }
    public Object Data { get; }
    public string ToJson();
}