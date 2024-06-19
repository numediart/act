using System;
using act_server.DataDescriptorClass;
using act_server.Enum;
using Microsoft.Extensions.Logging;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace act_server.Behavior;

public class ConnectionBehavior : WebSocketBehavior
{
    private EventManagerService _eventManagerService;

    public void Initialize(EventManagerService eventManagerService)
    {
        _eventManagerService = eventManagerService;
    }

    public Guid ClientId { get; set; }

    protected override void OnOpen()
    {
        base.OnOpen();
        ClientId = Guid.NewGuid();
        _eventManagerService.Emit(EnumEvents.ClientConnected.Name,
            "Client Connected Successfully",
            base.ID); // send event to event manager that client is connected
        //  Send($"Client connected: {ClientId}");
        
        Console.WriteLine(base.ID);
        
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);

        WebsocketMessage message = Newtonsoft.Json.JsonConvert.DeserializeObject<WebsocketMessage>(e.Data)!;
        _eventManagerService.Emit(EnumEvents.FromName(message.EventName).Name, message.Data ?? null, base.ID);
    }

    protected override void OnClose(CloseEventArgs e)
    {
        base.OnClose(e);
        _eventManagerService.Emit(EnumEvents.ClientDisconnected.Name,
            "Client Disconnected Successfully",
            ClientId.ToString()); // send event to event manager that client is disconnected 
        // No need to send message to client as it is already disconnected cause result in error
        // but can emit to room that client is disconnected
    }
}