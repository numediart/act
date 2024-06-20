using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class prefabRoom : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Owner;
    public TMP_Text Id;
    public TMP_InputField Password;
    
    
    public void SetRoomName(string name)
    {
        Name.text = name;
    }
    
    public void SetRoomOwner(string owner)
    {
        Owner.text = owner;
    }
    
    public void SetRoomId(string id)
    {
        Id.text = id;
    }
}
