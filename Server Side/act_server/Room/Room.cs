using System;
using System.Collections.Generic;

namespace act_server.Room;

public abstract class Room: IDisposable
{
    protected List<Client.Client> _clients;
    protected Guid _roomId;
    protected string? _password; // password for private room
    protected string _roomName;
    protected string _roomOwner; // client id of the room owner (creator)

    public string RoomOwner
    {
        get => _roomOwner;
    }

    public string RoomName
    {
        get => _roomName;
    }

    public Guid RoomId
    {
        get => _roomId;
    }


    public List<Client.Client> Clients => _clients;
    
    public bool HasPassword()
    {
        return _password != null;
    }
    
    public override string ToString()
    {
        return _roomId.ToString();
    }

    public abstract void InitRoom(string roomOwner, string? roomName, string? password = null);
    public abstract void AddClient(Client.Client client);
    public abstract void RemoveClient(Client.Client client);

    public abstract void Emit(string eventName, string data, string clientId);

    public abstract void Emit(string message, string clientId);
    public abstract void BroadcastToRoom(string message);
    
    public abstract void Dispose();
}