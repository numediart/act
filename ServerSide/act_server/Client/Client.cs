using System;
using act_server.DataDescriptorClass;

namespace act_server.Client;

public class Client
{
    private Guid _clientId;
    private string _sessionId;
    private WebSocketSharp.Server.WebSocketSessionManager _clientSocket;

    public override string ToString()
    {
        return _clientId.ToString();
    }

    public Guid ClientId
    {
        get => _clientId;
    }

    public WebSocketSharp.Server.WebSocketSessionManager ClientSocket
    {
        get => _clientSocket;
    }

    public Guid RoomId { get; set; }

    public Client(Guid clientId, WebSocketSharp.Server.WebSocketSessionManager clientSocket, string sessionId,
        string? username = null)
    {
        _clientId = clientId;
        _clientSocket = clientSocket;
        _sessionId = sessionId;
    }


    public void Emit(string eventName, object? data)
    {
        string message = Newtonsoft.Json.JsonConvert.SerializeObject(new WebsocketMessage(eventName, data));
        //Console.WriteLine(message);
        this._clientSocket.SendTo(message, _sessionId);
    }

    public void Emit(string message)
    {
        this._clientSocket.SendTo(message, _sessionId);
    }

    public void EmitAsync(string eventName, object? data, Action<bool> completionCallback)
    {
        string message = Newtonsoft.Json.JsonConvert.SerializeObject(new WebsocketMessage(eventName, data));
        //Console.WriteLine(message);
        this._clientSocket.SendToAsync(message, _sessionId, completionCallback);
    }
}