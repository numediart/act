namespace _Scripts._Version_1._0.Managers.Network
{
   using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Version_1._0;
using _Scripts._Version_1._0.Managers.Network.WebSocket;
using _Scripts._Version_1._0.Managers.Network.WebSocket.Enum;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivestreamingRoomManager : AbstractRoomSelection<List<LivestreamingRoom>, LivestreamingRoom>
{
    [Header("Instances")] [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private Transform _roomParent;
    [SerializeField] private GameObject _roomCreationGo;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_InputField _roomPasswordInputField;
    [SerializeField] private TMP_InputField _roomConfPasswordInputField;

    [Header("Parameters")] [SerializeField]
    private KeyCode _keyToToggleRoomCreationMenu;

    private List<LivestreamingRoom> _rooms = new List<LivestreamingRoom>();

    struct RoomInfos
    {
        public string RoomName;
        public string RoomOwner;
        public string RoomId;
        public bool HasPassword;
        public int ClientCount;
    }

    private void Awake()
    {
        StartCoroutine(RequestRoomInfos());
        EventManager.Instance.On(EnumEvents.LiveStreamingRoomsInfos.Name, data =>
        {
            List<RoomData> roomsData = JsonConvert.DeserializeObject<List<RoomData>>(data);
            if (roomsData.Count == 0)
                return;
            foreach (var roomData in roomsData)
            {
                var room = Instantiate(_roomPrefab, _roomParent);
                room.AddComponent<LivestreamingRoom>();
                room.GetComponent<LivestreamingRoom>().SetRoomName(roomData.RoomName);
                room.GetComponent<prefabRoom>().SetRoomName(roomData.RoomName);
                room.GetComponent<prefabRoom>().roomType = RoomType.Live;
                room.GetComponent<LivestreamingRoom>().SetRoomOwner(roomData.RoomOwner);
                room.GetComponent<prefabRoom>().SetRoomOwner(roomData.RoomOwner);
                room.GetComponent<LivestreamingRoom>().SetRoomId(roomData.RoomId);
                room.GetComponent<prefabRoom>().SetRoomId(roomData.RoomId);
                if (_rooms.Find(x => x.RoomId == roomData.RoomId) == null)
                {
                    _rooms.Add(room.GetComponent<LivestreamingRoom>());
                    room.transform.SetParent(_roomParent);
                }
                else
                {
                    Destroy(room);
                }
            }
        });
    }

    private readonly int _timeBetweenRequest = 5; // seconds

    private void Update()
    {
        if (Input.GetKeyDown(_keyToToggleRoomCreationMenu))
        {
            ToggleRoomCreationMenu();
        }
    }

    private IEnumerator RequestRoomInfos()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeBetweenRequest);
                NetworkManager.Instance.GetLivestreamingRooms();
                yield return null;
        }
    }
    
    public void ToggleRoomCreationMenu()
    {
        _roomCreationGo.SetActive(!_roomCreationGo.activeSelf);
    }

    public void HideRoomCreationMenu()
    {
        _roomCreationGo.SetActive(false);
    }

    public struct RoomData
    {
        public string RoomName;
        public string RoomOwner;
        public string RoomId;
        public bool HasPassword;
        public int ClientCount;
    }

    public override void CreateRoom()
    {
        var roomName = _roomNameInputField.text;
        var roomPassword = _roomPasswordInputField.text;
        var roomConfPassword = _roomConfPasswordInputField.text;
        if (roomPassword == roomConfPassword)
        {
            NetworkManager.Instance.CreateLiveStreamingRoom(roomName, roomPassword);
            EventManager.Instance.On(EnumEvents.LiveStreamingRoomCreated.Name, LivestreamingRoomCreated);
        }
        else
        {
            Debug.LogError("Passwords do not match");
        }
    }

    private void LivestreamingRoomCreated(string data)
    {
        _roomCreationGo.SetActive(false);
        RoomData roomData = JsonConvert.DeserializeObject<RoomData>(data);
        EventManager.Instance.Off(EnumEvents.LiveStreamingRoomCreated.Name, LivestreamingRoomCreated);
        LivestreamingRoom livestreamingRoom = NetworkManager.Instance.AddComponent<LivestreamingRoom>();
        livestreamingRoom.SetRoomName(_roomNameInputField.text);
        livestreamingRoom.SetRoomId(roomData.RoomId);
        livestreamingRoom.SetRoomOwner(roomData.RoomOwner);
        SceneManager.LoadScene("Scenes/_Ver1.0 - LiveStream With LipSync");
    }

    public override void DeleteRoom(string roomName, string roomOwner, string password, string id)
    {
        _rooms.Remove(_rooms.Find(x =>
            x.RoomName == roomName && x.RoomOwner == roomOwner && x.RoomPassword == password && x.RoomId == id));
    }

    public override void DeleteAllRooms()
    {
        _rooms.Clear();
    }

    public static void JoinRoom(string password, string id, GameObject roomPrefab)
    {
        NetworkManager.Instance.JoinLiveStreamingRoom(id, password);
        EventManager.Instance.On(EnumEvents.LivestreamingRoomJoined.Name, data =>
        {
            Debug.Log("Joining room");
            LivestreamingRoom livestreamingRoom = NetworkManager.Instance.AddComponent<LivestreamingRoom>();
            livestreamingRoom.SetRoomId(id);
            EventManager.Instance.Off(EnumEvents.LivestreamingRoomJoined.Name, data => { });
            SceneManager.LoadScene("Scenes/_Ver1.0 - LiveStream With LipSync");
        });
    }

    public override List<LivestreamingRoom> GetRooms()
    {
        return _rooms;
    }
}
}