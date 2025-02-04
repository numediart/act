#nullable enable
using System;
using act_server.Enum;
using act_server.Interface;

namespace act_server.DataDescriptorClass
{

    public class WebsocketMessage : IWebsocketPayload
    {
        public WebsocketMessage(string eventName, object? data)
        {
            Data = data;
            EventName = EnumEvents.FromName(eventName).Name;
        }
        public object? Data { get; set; }

        public string EventName { get; }


        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}