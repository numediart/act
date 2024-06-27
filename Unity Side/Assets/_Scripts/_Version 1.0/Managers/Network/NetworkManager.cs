using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Version_1._0;
using _Scripts._Version_1._0.Controllers.RoomController;
using _Scripts._Version_1._0.Managers.Network.WebSocket;
using _Scripts._Version_1._0.Managers.Network.WebSocket.Enum;
using _Scripts._Version_1._0.Services.RoomServices.LiveStreamingRoomService;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class NetworkManager : MonoBehaviour
{
    #region Singleton

    public static NetworkManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [Header("User")] public string UserId;
    public bool IsAdmin;
    public string CurrentRoomId;

    [Header("Parameters")] [SerializeField]
    private NetworkMode _networkMode;

    [Header("Menu Scene")] public string M_MenuManagerIdentifier;

    [Header("Room Selection Scene")] public string RS_MenuManagerIdentifier;
    public string RoomManagerIdentifier;

    [Header("Admin or User Scene")] public string RoomInterfaceControllerIdentifier;

    private RoomInterfaceController _roomInterfaceController;


    #region Intern Variables

    // References
    private MenuManager _menuManager;
    private WoZRoomManager _roomManager;


    // Room Types
    private string _roomSelectionType = "Room_Selection";
    private string _wozRoomType = "Room_Wizard_Of_Oz";

    // Room IDs
    private string _mainRoomId = "UMONS_Avatar_Lobby";

    public static string currentRoomId;

    #endregion

    private void LookForScriptRefOnSceneChange(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (_menuManager == null)
        {
            try
            {
                _menuManager = GameObject
                    .Find(scene.name == "_Ver 1.0 - Menu" ? M_MenuManagerIdentifier : RS_MenuManagerIdentifier)
                    .GetComponent<MenuManager>();
            }
            catch (Exception e)
            {
                Debug.Log("_menuManager not found in that scene");
            }
        }

        if (_roomManager == null)
        {
            try
            {
                _roomManager = GameObject.Find(RoomManagerIdentifier).GetComponent<WoZRoomManager>();
            }
            catch (Exception e)
            {
                Debug.Log("_roomManager not found in that scene");
            }
        }

        if (_roomInterfaceController == null)
        {
            try
            {
                _roomInterfaceController = GameObject.Find(RoomInterfaceControllerIdentifier)
                    .GetComponent<RoomInterfaceController>();
            }
            catch (Exception e)
            {
                Debug.Log("_roomInterfaceController not found in that scene");
            }
        }
    }

    private async void Start()
    {
        // Toggle event when scene is changing
        SceneManager.sceneLoaded += LookForScriptRefOnSceneChange;

        // Look for script ref in current scene
        LookForScriptRefOnSceneChange(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        Application.runInBackground = true;
        WebsocketManager webSocketManager = WebsocketManager.Instance;
        await webSocketManager.Connect("ws://localhost:9001");
        // User ID is the clientID 
    }


    public async void CreateWoZRoom(string roomName, [CanBeNull] string password)
    {
        // Send request to server
        await WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestWoZRoomCreation.Name, Data = new { RoomName = roomName, Password = password }
        }));
    }

    public async void JoinWoZRoom(string id, string password)
    {
        await WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestWoZRoomJoin.Name, Data = new { RoomId = id, Password = password }
        }));
    }

    public async void GetWoZRooms()
    {
        // Send request to server
        await WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestWoZRooms.Name, Data = new { }
        }));
    }

    public async void CreateLiveStreamingRoom(string roomName, [CanBeNull] string password)
    {
        // Send request to server
        await WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestLiveStreamingRoom.Name,
            Data = new { roomName = roomName, password = password }
        }));
    }

    public async void GetLivestreamingRooms()
    {
        await WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestLiveStreamingRooms.Name, Data = new { }
        }));
    }

    public async void JoinLiveStreamingRoom(string id, string password)
    {
        // Send request to server
        await WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestLiveStreamingRoom.Name, Data = new { RoomId = id, Password = password }
        }));
    }

    public async void RequestRoomInfo(string roomId)
    {
        // Send request to server
        await WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestRoomInfo.Name, Data = new { roomId = roomId }
        }));
    }

    public async void RequestPasswordChange(string roomId, string oldPassword, string newPassword)
    {
        // Send request to server
        await WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestPasswordChange.Name,
            Data = new { roomId = roomId, oldPassword = oldPassword, newPassword = newPassword }
        }));
    }

    public struct AvatarHeadMoveData
    {
        public string roomId;
        public double x;
        public double y;
        public double z;

        public AvatarHeadMoveData(string roomId, double x, double y, double z)
        {
            this.roomId = roomId;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public object ToJson()
        {
            return new { roomId, x, y, z };
        }
    }

    public async void AvatarHeadMoved(double poseRx, double poseRy, double poseRz)
    {
        // Send request to server
        if (Instance.TryGetComponent<WoZRoom>(out var woZRoom) == false)
            return;
        woZRoom = Instance.GetComponent<WoZRoom>();
        AvatarHeadMoveData data = new AvatarHeadMoveData(woZRoom.RoomId, poseRx, poseRy, poseRz);
        Debug.Log(JsonConvert.SerializeObject(data.ToJson()));
        Debug.Log(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestAvatarHeadMove.Name, Data = data.ToJson()
        }));
        WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestAvatarHeadMove.Name, Data = data.ToJson()
        }));
    }

    public async void AvatarPoseTransitionToNewFrame(string newRot, float durationInSeconds)
    {
        if (Instance.TryGetComponent<WoZRoom>(out var woZRoom) == false)
            return;
        woZRoom = Instance.GetComponent<WoZRoom>();
        WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestAvatarPoseTransition.Name,
            Data = new
            {
                roomId = woZRoom.RoomId, AvatarPoseTransitionData = newRot, durationInSeconds = durationInSeconds
            }
        }));
    }

    public async void AvatarBlendShapesMoved(string dictOfBlendShapes)
    {
        if (Instance.TryGetComponent<WoZRoom>(out var woZRoom) == false)
            return;
        woZRoom = Instance.GetComponent<WoZRoom>();
        WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestAvatarBlendshapeMove.Name,
            Data = new { roomId = woZRoom.RoomId, BlendshapeDict = dictOfBlendShapes, Value = "" }
        }));
    }

    public struct AvatarBlendshapeTransitionData
    {
        public string BlendshapeDict;
        public string Value;
        public string Duration;
    }

    public async void AvatarBlendShapeTransitionToNewFrame(string changesDict, float durationInSeconds)
    {
        if (Instance.TryGetComponent<WoZRoom>(out var woZRoom) == false)
            return;
        woZRoom  = Instance.GetComponent<WoZRoom>();
        WebsocketManager.Instance.Send(JsonConvert.SerializeObject(new
        {
            EventName = EnumEvents.RequestAvatarBlendshapeTransition.Name,
            Data = new
            {
                roomId = woZRoom.RoomId,
                blendshapeTransitionData = new AvatarBlendshapeTransitionData
                    { BlendshapeDict = changesDict, Value = changesDict, Duration = durationInSeconds.ToString() }
            }
        }));
    }


/*
    private void RegisterEvents()
    {



    }
    // Get answer from server
    private void FixedUpdate()
    {
        foreach (Message m in _msgList)
        {
            switch (m.Type)
            {
                case "UserConnected":
                    FeedbackManager.Instance.CreateFeedBack("New user connected: " + m.GetString(0),
                        FeedbackType.INFORMATION);
                    break;

                #region Woz Room : ADMIN & USER
                case "AskForUserInformation":
                    _pioconnectionWoz.Send("SendUserInfos", IsAdmin);
                    break;
                case "AvatarHeadMove":
                    if (IsAdmin)
                    {
                        throw new Exception("Admin got an order of avatar head movement");
                    }
                    MainManager.Instance.HeadPoseController.HeadPoseUpdateByValue(m.GetDouble(0), m.GetDouble(1), m.GetDouble(2));
                    break;
                case "AvatarBlendShapesMove":
                    if (IsAdmin)
                    {
                        throw new Exception("Admin got an order of avatar key movement");
                    }
                    MainManager.Instance.BlendShapesController.ChangeBlendShapesByDict(JsonConvert.DeserializeObject<Dictionary<string, double>>(m.GetString(0)));
                    break;
                case "AvatarBlendShapesTransition":
                    if (IsAdmin)
                    {
                        throw new Exception("Admin got an order of AvatarBlendShapesTransition");
                    }
                    MainManager.Instance.BlendShapesController.TransitionToDict(JsonConvert.DeserializeObject<Dictionary<string, double>>(m.GetString(0)),
                        m.GetFloat(1));
                    break;
                case "AvatarPoseTransition":
                    if (IsAdmin)
                    {
                        throw new Exception("Admin got an order of AvatarPoseTransition");
                    }
                    MainManager.Instance.HeadPoseController.MakeRotTransition(JsonConvert.DeserializeObject<Vector3>(m.GetString(0)),
                        m.GetFloat(1));
                    break;
                #endregion

                #region Selection Room
                case "AcceptRoomJoinRequest":
                    if (m.GetBoolean(0))
                        JoinRoomAsAdmin(m.GetString(1));
                    else
                        JoinRoomAsUser(m.GetString(1));
                    break;
                case "RefuseRoomJoinRequest":
                    RefuseRoomJoin(m.GetString(0));
                    break;
                case "AcceptRoomCreationRequest":
                    CreateMyNewRoom(m.GetString(0));
                    break;
                case "RefuseRoomCreationRequest":
                    RefuseMyRoomCreation();
                    break;
                case "NewRoomAdded":
                    CreateNewRoom(m.GetString(0));
                    break;
                case "AcceptPasswordModificationRequest":
                    FeedbackManager.Instance.CreateFeedBack("Room password changed", FeedbackType.SUCCESS);
                    break;
                case "RefusePasswordModificationRequest":
                    FeedbackManager.Instance.CreateFeedBack("Password modification denied: " + m.GetString(0)
                        , FeedbackType.ERROR);
                    break;
                case "RoomAvailabilityChanged":
                    FeedbackManager.Instance.CreateFeedBack("Room availability successfully changed",
                        FeedbackType.SUCCESS);
                    break;
                case "RoomAvailabilityNotChanged":
                    FeedbackManager.Instance.CreateFeedBack("Room availability not changed" + m.GetString(0)
                        , FeedbackType.ERROR);
                    break;
                case "RequestRoomInfo":
                    _roomInterfaceController.UpdateRoomInfos(m.GetBoolean(0));
                    break;
                #endregion
            }
        }

        _msgList.Clear();
    }

    // Handle server messages
    public void HandleMessage(object sender, Message m) {
        _msgList.Add(m);
    }

    private void ConnectToRoom(string roomName, string roomType)
    {
        PlayerIO.Authenticate(
            "act-i1npa6ywesmm33kf7hwmq",            //ID provided by the Numediart's Player.io account
            "public",                               //Your connection id
            new Dictionary<string, string> {        //Authentication arguments
                { "userId", UserId },
            },
            null,                                   //PlayerInsight segments
            delegate (Client client) {
                Debug.Log("Successfully connected to Player.IO");

                Debug.Log("CreateJoinRoom");
                //Create or join the room
                client.Multiplayer.CreateJoinRoom(
                    roomName,                    //Room id. If set to null a random roomid is used
                    roomType,                   //The room type started on the server
                    true,                               //Should the room be visible in the lobby?
                    null,
                    null,
                    delegate (Connection connection) {
                        // We successfully joined a room so set up the message handler
                        if (roomType == _roomSelectionType)
                        {
                            _pioconnectionRoom = connection;
                            _pioconnectionRoom.OnMessage += HandleMessage;
                        }
                        else if (roomType == _wozRoomType)
                        {
                            _pioconnectionWoz = connection;
                            _pioconnectionWoz.OnMessage += HandleMessage;
                        }
                        else
                        {
                            throw new Exception("Unknown room type: " + roomType);
                        }

                        FeedbackManager.Instance.CreateFeedBack("Successfully joined room " + roomName, FeedbackType.SUCCESS);
                    },
                    delegate (PlayerIOError error) {
                        Debug.Log("Error Joining Room: " + error.ToString());
                    }
                );
            },
            delegate (PlayerIOError error) {
                Debug.Log("Error connecting: " + error.ToString());
            }
        );
    }

    #region Server Functions called externally

    public void RequestToJoinRoom(string roomId, string roomPassword, bool joinAsAdmin)
    {
        _pioconnectionRoom.Send("RoomJoinRequest", roomId, roomPassword, joinAsAdmin);
    }

    public void RequestToCreateRoom(string roomId, string roomPassword)
    {
        _pioconnectionRoom.Send("RoomCreationRequest", roomId, roomPassword);
    }

    public void UserSelectedView(bool isAdminView)
    {
        _pioconnectionWoz.Send("ChoosedView", isAdminView);
    }

    public void AvatarHeadMoved(double poseRx, double poseRy, double poseRz)
    {
        if (!IsAdmin)
        {
            throw new Exception("Non-admin could send avatar key move");
        }

        _pioconnectionWoz.Send("AvatarHeadMove", poseRx, poseRy, poseRz);
    }

    public void AvatarBlendShapesMoved(string dictOfBlendShapes)
    {
        if (!IsAdmin)
        {
            throw new Exception("Non-admin could send avatar key move");
        }

        _pioconnectionWoz.Send("AvatarBlendShapesMove", dictOfBlendShapes);
    }

    public void AvatarBlendShapeTransitionToNewFrame(string changesDict, float durationInSeconds)
    {
        if (!IsAdmin)
        {
            throw new Exception("Non-admin could send avatar transition");
        }

        _pioconnectionWoz.Send("AvatarBlendShapesTransition", changesDict, durationInSeconds);
    }

    public void AvatarPoseTransitionToNewFrame(string newRot, float durationInSeconds)
    {
        if (!IsAdmin)
        {
            throw new Exception("Non-admin could send avatar transition");
        }
        _pioconnectionWoz.Send("AvatarPoseTransition", newRot, durationInSeconds);
    }

    public void DisconnectFromRoomEvent()
    {
        _pioconnectionWoz.Disconnect();

        _pioconnectionRoom.Send("PlayerDisconnected", CurrentRoomId, IsAdmin);

        _menuManager.LoadRoomSelectionScene();

        // Erase user infos
        IsAdmin = false;
        CurrentRoomId = "";
    }

    public void RequestRoomPasswordModification(string oldPassword, string newPassword)
    {
        if (!IsAdmin)
        {
            throw new Exception("Non-Admin could send password modif request");
        }

        _pioconnectionRoom.Send("PasswordModificationRequest", CurrentRoomId, oldPassword, newPassword);
    }

    public void ChangeRoomAvailabilityEvent(bool isRoomAvailable)
    {
        if (!IsAdmin)
        {
            throw new Exception("Non-Admin could change room availability");
        }

        _pioconnectionRoom.Send("ChangeRoomAvailability", CurrentRoomId, isRoomAvailable);
    }

    public void RequestRoomInfos()
    {
        _pioconnectionRoom.Send("RequestRoomInfo", CurrentRoomId);
    }

    public void ReturnToMainMenuEvent()
    {
        if (_pioconnectionWoz != null)
            _pioconnectionWoz.Disconnect();

        if (_pioconnectionRoom != null)
            _pioconnectionRoom.Disconnect();
    }

    public void ConnectToRoomSelection()
    {
        ConnectToRoom(_mainRoomId, _roomSelectionType);
    }

    #endregion

    #region Room

    // Join
    private void JoinRoomAsAdmin(string roomName)
    {
        _pioconnectionRoom.Send("AdminJoinedRoom", roomName);

        ConnectToRoom(roomName, _wozRoomType);

        // Update user infos
        IsAdmin = true;
        CurrentRoomId = roomName;

        _roomManager.ResetRoomsDict();

        _menuManager.LoadAdminScene();
    }

    private void JoinRoomAsUser(string roomName)
    {
        _pioconnectionRoom.Send("UserJoinedRoom", roomName);

        ConnectToRoom(roomName, _wozRoomType);

        // Update user infos
        IsAdmin = false;
        CurrentRoomId = roomName;

        _roomManager.ResetRoomsDict();

        _menuManager.LoadUserScene();
    }

    private void RefuseRoomJoin(string refuseMessage)
    {
        FeedbackManager.Instance.CreateFeedBack("Refused room join: " + refuseMessage, FeedbackType.ERROR);
    }

    // Creation
    private void CreateMyNewRoom(string roomName)
    {
        _roomManager.CreateNewRoom(roomName);
        _roomManager.HideRoomCreationMenu();

        FeedbackManager.Instance.CreateFeedBack("Room successfully created", FeedbackType.SUCCESS);
    }

    private void RefuseMyRoomCreation()
    {
        FeedbackManager.Instance.CreateFeedBack("Room couldn't be added", FeedbackType.ERROR);
    }

    private void CreateNewRoom(string roomName)
    {
        _roomManager.CreateNewRoom(roomName);
        FeedbackManager.Instance.CreateFeedBack("New room added: " + roomName, FeedbackType.INFORMATION);
    }

    #endregion
    */
}