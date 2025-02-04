#nullable enable
using System;
using act_server.Enum;
using act_server.Interface;

namespace act_server.DataDescriptorClass
{

    public class WebsocketMessage(string eventName, object? data) : IWebsocketPayload
    {
        public object? Data { get; set; } = data;

        public string EventName { get; } = EnumEvents.FromName(eventName).Name;


        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}