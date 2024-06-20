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

public class WoZRoomManager : AbstractRoomSelection<List<WoZRoom>, WoZRoom>
{
    [Header("Instances")] 
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private Transform _roomParent;
    [SerializeField] private GameObject _roomCreationGo;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_InputField _roomPasswordInputField;
    [SerializeField] private TMP_InputField _roomConfPasswordInputField;
    
    [Header("Parameters")] 
    [SerializeField] private KeyCode _keyToToggleRoomCreationMenu;
    
    private List<WoZRoom> _rooms = new List<WoZRoom>();
    
    
    private void Update()
    {
        if (Input.GetKeyDown(_keyToToggleRoomCreationMenu))
        {
            ToggleRoomCreationMenu();
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
        public string Password;
        public string RoomId;
        public bool HasPassword;
        public bool isAdmin;
    }
    public override void CreateRoom()
    {
        var roomName = _roomNameInputField.text;
        var roomPassword = _roomPasswordInputField.text;
        var roomConfPassword = _roomConfPasswordInputField.text;
        if (roomPassword == roomConfPassword)
        {
            var room = Instantiate(_roomPrefab, _roomParent);
            NetworkManager.Instance.CreateWoZRoom(roomName,roomPassword);
            EventManager.Instance.On(EnumEvents.WoZRoomCreated.Name, (string data) =>
            {
                RoomData newRoomData = JsonConvert.DeserializeObject<RoomData>(data);
                room.GetComponent<WoZRoom>().SetRoomName(newRoomData.RoomName);
                room.GetComponent<prefabRoom>().SetRoomName(newRoomData.RoomName);
                room.GetComponent<WoZRoom>().SetRoomOwner(newRoomData.RoomOwner);
                room.GetComponent<prefabRoom>().SetRoomOwner(newRoomData.RoomOwner);
                room.GetComponent<WoZRoom>().SetRoomPassword(newRoomData.Password);
                room.GetComponent<WoZRoom>().SetRoomId(newRoomData.RoomId);
                room.GetComponent<prefabRoom>().SetRoomId(newRoomData.RoomId);
                _rooms.Add(room.GetComponent<WoZRoom>());
            });
        }
        else
        {
            Debug.LogError("Passwords do not match");
        }
    }

    public override void DeleteRoom(string roomName, string roomOwner, string password , string id)
    {
        _rooms.Remove(_rooms.Find(x => x.RoomName == roomName && x.RoomOwner == roomOwner && x.RoomPassword == password && x.RoomId == id));
    }

    public override void DeleteAllRooms()
    {
        _rooms.Clear();
    }

    public override void JoinRoom(string roomName, string roomOwner, string password , string id)
    {
        throw new NotImplementedException();
    }

    public override List<WoZRoom> GetRooms()
    {
        return _rooms;
    }
}
