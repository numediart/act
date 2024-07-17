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

public class WoZRoomManager : AbstractRoomSelection<List<WoZRoom>, WoZRoom>
{
    [Header("Instances")] [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private Transform _roomParent;
    [SerializeField] private GameObject _roomCreationGo;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_InputField _roomPasswordInputField;
    [SerializeField] private TMP_InputField _roomConfPasswordInputField;

    [Header("Parameters")] [SerializeField]
    private KeyCode _keyToToggleRoomCreationMenu;

    private List<WoZRoom> _rooms = new List<WoZRoom>();

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
        // request for room WoZ room info
        EventManager.Instance.On(EnumEvents.WoZRoomsInfos.Name, data =>
        {
            List<RoomData> roomsData = JsonConvert.DeserializeObject<List<RoomData>>(data);
            if (roomsData.Count == 0)
                return;
            foreach (var roomData in roomsData)
            {
                var room = Instantiate(_roomPrefab, _roomParent);
                room.AddComponent<WoZRoom>();
                room.GetComponent<WoZRoom>().SetRoomName(roomData.RoomName);
                room.GetComponent<prefabRoom>().SetRoomName(roomData.RoomName);
                room.GetComponent<WoZRoom>().SetRoomOwner(roomData.RoomOwner);
                room.GetComponent<prefabRoom>().roomType = RoomType.WoZ;
                room.GetComponent<prefabRoom>().SetRoomOwner(roomData.RoomOwner);
                room.GetComponent<WoZRoom>().SetRoomId(roomData.RoomId);
                room.GetComponent<prefabRoom>().SetRoomId(roomData.RoomId);
                if (_rooms.Find(x => x.RoomId == roomData.RoomId) == null)
                {
                    _rooms.Add(room.GetComponent<WoZRoom>());
                    room.transform.SetParent(_roomParent);
                }
                else
                {
                    Destroy(room);
                }
            }

            _lastRequestTime = Time.time;
        });
    }

    private readonly int _timeBetweenRequest = 5; // seconds
    private float _lastRequestTime = 0;

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
                NetworkManager.Instance.GetWoZRooms();
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
            NetworkManager.Instance.CreateWoZRoom(roomName, roomPassword);
            EventManager.Instance.On(EnumEvents.WoZRoomCreated.Name, WoZRoomCreated);
        }
        else
        {
            Debug.LogError("Passwords do not match");
        }
    }

    private void WoZRoomCreated(string data)
    {
        _roomCreationGo.SetActive(false);
        RoomData roomData = JsonConvert.DeserializeObject<RoomData>(data);
        EventManager.Instance.Off(EnumEvents.WoZRoomCreated.Name, WoZRoomCreated);
        WoZRoom woZRoom = NetworkManager.Instance.AddComponent<WoZRoom>();
        woZRoom.SetRoomName(_roomNameInputField.text);
        woZRoom.SetRoomId(roomData.RoomId);
        woZRoom.SetRoomOwner(roomData.RoomOwner);
        SceneManager.LoadScene("_Ver 1.0 - Admin");
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
        NetworkManager.Instance.JoinWoZRoom(id, password);
        EventManager.Instance.On(EnumEvents.WoZRoomJoined.Name, data =>
        {
            Debug.Log("Joining room");
            WoZRoom woZRoom = NetworkManager.Instance.AddComponent<WoZRoom>();
            woZRoom.SetRoomId(id);
            EventManager.Instance.Off(EnumEvents.WoZRoomJoined.Name, data => { });
            SceneManager.LoadScene("_Ver 1.0 - User");
        });
    }

    public override List<WoZRoom> GetRooms()
    {
        return _rooms;
    }
}