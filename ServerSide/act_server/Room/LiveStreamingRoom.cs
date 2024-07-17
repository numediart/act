using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using act_server.Controller;
using act_server.DataDescriptorClass;
using act_server.Enum;
using act_server.Utils;
using act_server.Utils.AU2BlendShapes;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;

namespace act_server.Room;

public class LiveStreamingRoom : Room
{

    ILogger<LiveStreamingRoom> _logger;
    string _actionUnitMessage;
    string _timestamp;

    public LiveStreamingRoom(ILogger<LiveStreamingRoom> logger)
    {
        _logger = logger;
        _roomId = Guid.NewGuid();
        _clients = new List<Client.Client>();
    }

    public static long UnixTimeNowMillisec()
    {
        DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        long unixTimeStampInTicks = (DateTime.UtcNow - unixStart).Ticks;
        long timeNowMs = unixTimeStampInTicks / (TimeSpan.TicksPerMillisecond / 10000); // 100ns
        return timeNowMs;
    }
    
    public struct AudioData
    {
        public long timestamp;
        public byte[] buffer;
        public int bytesRecorded;
        public string roomId;
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
    
    public override void InitRoom(string roomOwner, string? roomName, string? password=null)
    {
        this._roomOwner = roomOwner;
        this._roomName = roomName;
        this._password = password;
        _logger.LogInformation($"Room {this._roomId} created by {this._roomOwner}");
        _logger.LogCritical("LiveStreamingRoom initialized");
        _timestamp = UnixTimeNowMillisec().ToString();
    }

    Task ReceiveMessage()
    {
        return Task.Run(() =>
        {
           /* while (true)
            {
               // var audioMessage = _audioSubscriberSocket.ReceiveFrameString();
                IncomingData obj = Newtonsoft.Json.JsonConvert.DeserializeObject<IncomingData>(message[2]);
                // _logger.LogInformation(message[2]);
                _clients.ForEach(client =>
                {
                    // Parse the message and send the blendshapes to the client
                  //  if (obj.timestamp >= long.Parse(_timestamp))
                  //  {
                        AU2BlendShapes au2BlendShapes = new AU2BlendShapes(obj);
                        client.Emit((new WebsocketMessage(
                            EnumEvents.LiveStreamingData.Name,
                            au2BlendShapes.ToString()).ToJson()));
                        string poseData = Newtonsoft.Json.JsonConvert.SerializeObject(obj.pose);
                        client.Emit((new WebsocketMessage(
                                EnumEvents.LiveStreamingAvatarHeadPose.Name,
                                poseData)
                            .ToJson()));
                        string eyeGazeData = Newtonsoft.Json.JsonConvert.SerializeObject(obj.gaze);
                        client.Emit((new WebsocketMessage(
                                EnumEvents.LiveStreamingAvatarEyeGaze.Name,
                                eyeGazeData)
                            .ToJson()));
                  //  }

                  /*  if (audioMessage != "openface_audio")
                    {
                        AudioData audioData = Newtonsoft.Json.JsonConvert.DeserializeObject<AudioData>(audioMessage);
                        
                            client.Emit((new WebsocketMessage(
                                    EnumEvents.LiveStreamingAudioData),
                                    audioMessage)
                                .ToJson()));
                        
                    }*/         
             //   });
            //}
        });
    }
    
    public override void AddClient(Client.Client client)
    {
        if (client.RoomId == this._roomId)
        {
            throw new System.Exception("Client already in room");
        }

        this._clients.Add(client);
    }

    public override void RemoveClient(Client.Client client)
    {
        if (client.RoomId != this._roomId)
        {
            throw new System.Exception("Client not in room");
        }

        this._clients.Remove(client);
        
        if(this._clients.Count == 0)
        {
            _logger.LogInformation($"Room {this._roomId} deleted");
            this.Dispose();
        }
    }

    public override void Emit(string eventName, string data, string clientId)
    {
        _clients.Find(client => client.ClientId.ToString() == clientId)?.Emit(eventName, data);
    }

    public override void Emit(string message, string clientId)
    {
        _clients.Find(client => client.ClientId.ToString() == clientId)?.Emit(message);
    }

    public override void BroadcastToRoom(string message)
    {
       _clients.ForEach(client => client.Emit(message));
    }

    public override void Dispose()
    {
        _clients.Clear();
    }
}