using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using act_server.Behavior;
using act_server.DataDescriptorClass;
using act_server.Enum;
using act_server.Room;
using Microsoft.Extensions.Logging;
using WebSocketSharp.Server;

namespace act_server.Service.MainWebSocketService;

public class MainWebSocketService
{
    private WebSocketServer _websocket;
    private EventManagerService _eventManager;
    private readonly ConcurrentDictionary<string, Client.Client> _clients = new ConcurrentDictionary<string, Client.Client>();
    private readonly List<Room.Room> _rooms = new List<Room.Room>();
    private readonly ConcurrentDictionary<string, Room.Room> _roomClients = new ConcurrentDictionary<string, Room.Room>();
    private const ushort _port = 9001;

    private ILogger<MainWebSocketService> logger;
    // Add this line

    
    public MainWebSocketService (ILogger<MainWebSocketService> logger, EventManagerService eventManagerService)
    {
        _websocket = new WebSocketServer($"ws://localhost:{_port}");
        _eventManager = eventManagerService;
        RegisterWebsocketService<ConnectionBehavior>("/");
        _websocket.Start();
        this.logger = logger;
        this.logger.LogInformation($"Server started at ws://localhost:{_port}");
    }
    
    public Client.Client GetClient(string clientId)
    {
        if (!_clients.TryGetValue(clientId, out var client))
        {
            logger.LogError($"Client with {clientId} not found");
        }

        return client;
    }

   
    public void RegisterWebsocketService<T>(string path) where T : WebSocketBehavior, new()
    {
        _websocket.AddWebSocketService(path, () =>
        {
            var behavior = new ConnectionBehavior();
            behavior.Initialize(_eventManager);
            return behavior;
        });
        OnConnection((s => logger.LogInformation(s)));
        OnDisconnect((s => logger.LogInformation(s)));
        //OnLiveStreamingRoomCreation();
    }
    
    public void UnregisterWebsocketService(string path)
    {
        _websocket.RemoveWebSocketService(path);
    }

    private bool TryGetClient(string clientId, out Client.Client client)
    {
        return _clients.TryGetValue(clientId, out client);
    }

    private bool TryGetRoom(string roomId, out Room.Room room)
    {
        return _roomClients.TryGetValue(roomId, out room);
    }

    private void BroadcastToRoom(Room.Room room, string message)
    {
        room?.BroadcastToRoom(message);
    }

    private void BroadcastToAll(string message)
    {
        _websocket.WebSocketServices["/"].Sessions.Broadcast(message);
    }

    private void EmitToClient(string clientId, string message)
    {
        _clients.TryGetValue(clientId, out var client);
        client?.Emit(message);
    }

    private void RemoveClientFromRoom(Client.Client client)
    {
        if (client.RoomId != Guid.Empty && _roomClients.TryGetValue(client.RoomId.ToString(), out var room))
        {
            room?.RemoveClient(client);
            if (room is { Clients.Count: 0 })
            {
                if(room is LiveStreamingRoom liveStreamingRoom)
                {
                    liveStreamingRoom.Dispose();
                    UnregisterWebsocketService("/openface/ActionUnit/" + room.RoomId);
                    UnregisterWebsocketService("/openface/Audio/" + room.RoomId);
                    UnregisterWebsocketService("/mediapipe/blendshapedata/" + room.RoomId);
                }
                _rooms.Remove(room);
            }
        }
    }

    public void OnConnection(Action<string> callback)
    {
        _eventManager.On(EnumEvents.ClientConnected.Name,
            (data, clientId) =>
            {
                WebSocketSessionManager? session = null;
                foreach (var path in _websocket.WebSocketServices.Paths)
                {
                    if (_websocket.WebSocketServices[path].Sessions.TryGetSession(clientId, out var outSession))
                    {
                        session = _websocket.WebSocketServices[path].Sessions;
                        break;
                    }
                }
                
                if (session == null)
                {
                    logger.LogError("Session not found");
                    return;
                }

                var client = new Client.Client(Guid.NewGuid(), session, clientId, data);
                _clients.TryAdd(clientId, client);
                session.SendTo(new WebsocketMessage(
                    EnumEvents.ClientConnected.Name,
                    "Client Connected Successfully, session :  " + clientId).ToJson(), clientId);
                callback(clientId);
            });
    }
    private void OnDisconnect(Action<string> callback)
    {
        _eventManager.On(EnumEvents.ClientDisconnected.Name,
            (data, clientId) =>
            {
                Console.WriteLine($"Client {clientId} disconnected");
                if (_clients.TryRemove(clientId, out var client))
                {
                    RemoveClientFromRoom(client);
                    callback(clientId);
                }
            });
        
    }
    public void OnLiveStreamingRoomCreation()
    {
        LiveStreamingRoom room = null;

        _eventManager.On(EnumEvents.RequestLiveStreamingRoom.Name,
            (data, clientId) =>
            {
                logger.LogInformation(_clients.Count.ToString());
                if (room == null)
                {
                    room = new LiveStreamingRoom(new Logger<LiveStreamingRoom>(null));
                }

                TryGetClient(clientId, out Client.Client client);
                logger.LogInformation(clientId);
                room.InitRoom(clientId, "Live Streaming Room", null);
                room.AddClient(client);
                _rooms.Add(room);
            });
    }
}