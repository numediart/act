using System;
using System.Collections.Generic;
using act_server.Service.RoomService.WoZRoomService;
using Microsoft.Extensions.Logging;

namespace act_server.Room;

public class WoZRoom:Room
{
    private readonly ILogger<WoZRoom> _logger;
    public WoZRoom(ILogger<WoZRoom> logger)
    {
        _roomId = Guid.NewGuid();
        _clients = new List<Client.Client>();
        _logger = logger;
        _password = null;
    }
    
    public override void InitRoom(string roomOwner, string? roomName, string? password = null)
    {
        _roomName = roomName;
        _roomOwner = roomOwner;
        _password = password;
    }

    public bool VerifyPassword(string password)
    {
        return _password == password;
    }
    public void ChangePassword(string newPassword)
    {
        _password = newPassword;
    }
    public bool CheckAdmin(string clientId)
    {
        return clientId == _roomOwner;
    }
    public override void AddClient(Client.Client client)
    {
        if (client.RoomId == RoomId)
        {
            _logger.LogError("Client already in room");
        }
        _clients.Add(client);
        client.RoomId = RoomId;
        _logger.LogInformation($"Client {client.ClientId} added to room {RoomId}");
    }

    public override void RemoveClient(Client.Client client)
    {
        if (_clients.Contains(client))
        {
            _clients.Remove(client);
            _logger.LogInformation($"Client {client.ClientId} removed from room {RoomId}");
        }
        else
        {
            _logger.LogError("Client not in room");
        }
    }

    public override void Emit(string eventName, string data,string clientId)
    {
        _logger.LogInformation($"Emitting message to client {clientId} data {eventName} {data} ");
        _clients.Find(client => client.ClientId.ToString() == clientId)?.Emit(eventName, data);
    }

    public override void Emit(string message, string clientId)
    {
        _logger.LogInformation($"Emitting message to client {clientId} data {message}");
        _clients.Find(client => client.ClientId.ToString() == clientId)?.Emit(message);
    }
  

    public override void BroadcastToRoom(string message)
    {
        foreach (var client in _clients)
        {
            client.Emit(message);
        }
    }

    public override string ToString()
    {
        return $"RoomId: {RoomId}, RoomName: {RoomName}, NumClients: {_clients.Count}, isPrivate: {_password != null}";
    }

    public override void Dispose()
    {
        _clients.Clear();
    }
    
}