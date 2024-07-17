using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Version_1._0.Managers.Network;
using TMPro;
using UnityEngine;

public enum RoomType
{
    WoZ,
    Live,
    Record
}
public class prefabRoom : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Owner;
    public TMP_Text Id;
    public TMP_InputField Password;
    
    
    public RoomType roomType;
    
    public void SetRoomName(string name)
    {
        Name.SetText(name);
        Name.text = name;
    }
    
    public void SetRoomOwner(string owner)
    {
        Owner.SetText(owner);
    }
    
    public void SetRoomId(string id)
    { 
        Id.SetText(id);
    }

    public void JoinRoom()
    {
        switch (roomType)
        {
            case RoomType.WoZ:
                WoZRoomManager.JoinRoom(Password.text, Id.text, gameObject);
                break;
            case RoomType.Live:
                LivestreamingRoomManager.JoinRoom(Password.text, Id.text, gameObject);
                break;
            default:
                throw new Exception("Room type not implemented");
        }
    }
    
}
