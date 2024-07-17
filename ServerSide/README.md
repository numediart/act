
# Websocket based Server-Client communication with the unity's Avatar control Toolkit (ACT)

## How to use
___
1. Clone the whole repository (unity side and server side)
2. install [Unity hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe) and download unity v2022.3.9f1
3. Open the unity project in Unity3D
4. Open the server side in your favorite IDE (I used Rider)
5. Run the server side
6. For the unity side, please refer to the README.md in the unity side folder
> [!WARNING]
> Make sure you have C# toolchain installed

## How it works
___
The server is based on the websocket library from WebsocketSharp. We customized the websocket part to be event based.
That means all events have to be known by the server and the client to be well dispatch otherwise it will throw an
error.

The server use dependency injection to inject the controller and the services. The controller is the one that will
handle the events and the services are the one that will handle the logic. It should be easy to add new events and new
services, each controller and services and rooms inherit from a base abstract class which define some generic methods.

The server and client use a room system to handle multiple clients. Each client is assigned to a room and can only send
information to the clients in the same room. The client must send a request event for any action it wants to do. The
server will then dispatch the event to the right controller and the controller will handle the event.

## Block Diagram

![](https://github.com/numediart/ACT/blob/ACT_experimental/ServerSide/assets/Block_diagram.png)

## How to add new events
___
1. Create a new event in the [EnumEvent.cs](act_server/Enum/EnumEvent.cs) file

```csharp
    public static readonly EnumEvent YourEventName= new EnumEvent(GetNextValue(), "YourEventName");
```

## How to add new type of Room
___
Current Room created :

- [Room.cs](act_server/Room/Room.cs) : Base abstract class for all rooms
- [WoZRoom.cs](act_server/Room/WoZRoom.cs) : Room for the Wizard of Oz experiment
- [LiveStreamingRoom.cs](act_server/Room/LiveStreamingRoom.cs) : Room for the live-streaming experiment

1. Create a new room class that inherit from the Room class

```csharp
    public class YourRoomName : Room
    {
        ILogger<YourRoomName> _logger;
        
        public YourRoomName(IServiceProvider provider) 
        {
            _logger = provider.GetRequiredService<ILogger<YourRoomName>>();// Get the logger not mandatory 
        }
        public override void InitRoom(string roomOwner, string? roomName, string? password = null){
            // Initialize the room
        }
        
        public override void AddClient(Client.Client client){
            // Add the client to the room
        }
            
        public override void RemoveClient(Client.Client client){
            // Remove the client from the room
        }

        public override void Emit(string eventName, string data, string clientId){
            // Emit the message to the client
        }

        public override void Emit(string message, string clientId){
            // Emit the message to the client
        }
        public override void BroadcastToRoom(string message){
            // Broadcast the message to all clients in the room
        }
    
        public override void Dispose(){
// Dispose the room
            
        }
        
    }
```

2. Create Service for the room

Current Room Service created :

- [WoZRoomService.cs](act_server/Room/WoZRoomService.cs) : Service for the Wizard of Oz experiment
- [LiveStreamingRoomService.cs](act_server/Room/LiveStreamingRoomService.cs) : Service for the live-streaming experiment
- [AbstractRoomService.cs](act_server/Room/AbstractRoomService.cs) : Base abstract class for all room services

```csharp
    public class YourRoomNameService(ILogger<YourRoomName> logger, MainWebSocketService.MainWebSocketService mainWebSocketService)
    : AbstractRoomService<YourRoomName>(logger)
    {
        ILogger<YourRoomNameService> _logger;
        
        public YourRoomNameService(IServiceProvider provider){
            _logger = provider.GetRequiredService<ILogger<YourRoomNameService>>();// Get the logger 
            }
        
         protected override ILogger<TRoom> Logger { get; set; }

        protected override Dictionary<string, TRoom> Rooms { get; set; }
        public override void OnRequestRoomCreate(RoomCreationData roomCreationData){
            // Create the room
        }
        public override void OnRequestRoomJoin( string roomId, string clientId, string? password = null){
            // Join the room
        }
        public override void OnRequestRoomLeave( string roomId, string clientId){
            // Leave the room
        }
        public override void OnRequestRoomBroadcast( string roomId, string message){
            // Broadcast the message to all clients in the room
        }
    
        public override void OnRequestPasswordChange( string roomId, string clientId, string newPassword){
            // Change the password of the room
        }
        public override void OnRequestRoomInfo( string roomId, string clientId){
            // Get the information of the room
        }
        public void YourCustomMethod(){
            // Your custom method
        }
    }
```

In the service, you can add your custom method that will be called by the controller. But It's mandatory to implement
the abstract methods.

3. Create a controller for the room

Current Room Controller created :

- [WoZRoomController.cs](act_server/Controller/WoZRoomController/WoZRoomController.cs) : Controller for the Wizard of Oz
  experiment
- [LiveStreamingRoomController.cs](act_server/Controller/LiveStreamingRoomController/LiveStreamingRoomController.cs) :
  Controller for the live-streaming experiment
- [AbstractRoomController.cs](act_server/Controller/AbstractRoomController.cs) : Base abstract class for all room
  controllers

```csharp
    public class YourRoomNameController : AbstractRoomController<YourRoomNameService,YourRoomName >
    {
       protected override EventManagerService? PEventManagerService { get; set; }
       protected override YourRoomNameService? PRoomService { get; set; }
        protected override ILogger<AbstractRoomController<YourRoomNameService, YourRoomName>>? PLogger
        {
            get;
            set;
        }

    public YourRoomNameController(IServiceProvider serviceProvider)
    {
        PEventManagerService = serviceProvider.GetRequiredService<EventManagerService>();
        PRoomService = serviceProvider.GetRequiredService<YourRoomNameService>();
        PLogger = serviceProvider.GetRequiredService<ILogger<YourRoomNameController>>();
        RegisterEvents();
    }
        
        protected override void RegisterEvent (){
            // Register the event
            
        } 
        
        public override void OnRequestRoomCreate(RoomCreationData roomCreationData){
            // Create the room
        }
        public override void OnRequestRoomJoin( string roomId, string clientId, string? password = null){
            // Join the room
        }
        public override void OnRequestRoomLeave( string roomId, string clientId){
            // Leave the room
        }
        public override void OnRequestRoomBroadcast( string roomId, string message){
            // Broadcast the message to all clients in the room
        }
    
        public override void OnRequestPasswordChange( string roomId, string clientId, string newPassword){
            // Change the password of the room
        }
        public override void OnRequestRoomInfo( string roomId, string clientId){
            // Get the information of the room
        }
        public void YourCustomMethod(){
            // Your custom method
        }
    }
```

In the controller, you can add your custom method that will be called by the server. But It's mandatory to implement the
abstract methods.

4. Register the room in the [Program.cs](act_server/Program.cs) file

```csharp
    builder.ConfigureServices((context, services) =>
    {
        services.AddSingleton<YourRoomName>();
        services.AddSingleton<YourRoomNameService>();
        services.AddSingleton<YourRoomNameController>();
    });
```

5. Add The controller in the main worker [MainWorker.cs](act_server/MainWorker/MainWorker.cs)
```csharp
 public class MainWorker: BackgroundService
    {
        private readonly MainWebSocketService _mainWebSocketService;
        private readonly ILogger<MainWorker> _logger;
        private readonly EventManagerService _eventManagerService;
        private readonly YourRoomNameController _yourRoomNameController;
        public MainWorker(MainWebSocketService mainWebSocketService,  EventManagerService eventManagerService,ILogger<MainWorker> logger, YourRoomNameController yourRoomNameController)
        {
            _mainWebSocketService = mainWebSocketService;
            _eventManagerService = eventManagerService;
            _logger = logger;
            _yourRoomNameController = yourRoomNameController;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
      }
```
## TODO 
___

1. Add a way to handle the disconnection of the client
2. Add a way to handle the reconnection of the client
3. Find a way to transmit roomId to openface or any other facial recognition software to get user expression using websocket
4. Add Error Handling and Error Event

##  How to contribute 
___
1. Fork the project
2. Create a new branch (`git checkout -b feature/featureName`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/featureName`)
5. Create a new Pull Request
6. Wait for the review and approval

## Contact 
___
- Kevin El Haddad - ACT Creator
- [Pierre-Luc MILLET](https://github.com/Pierre-LucM)- pierre-luc.millet@student.junia.com - ACT Dev
- [Arthur PINEAU](https://github.com/Arthur-P0) - arthur.pineau@student.junia.com - ACT Dev
- [Armand DEFFRENNES](https://github.com/JambonPasFrais) - armand.deffrennes@student.junia.com - ACT Dev
