using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLipSync : MonoBehaviour
{
    public GameObject targetComponent;
    
    public void Toggle()
    {
        targetComponent.SetActive(!targetComponent.activeSelf);
    }
}
