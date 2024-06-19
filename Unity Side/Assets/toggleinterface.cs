using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleinterface : MonoBehaviour
{
    [SerializeField] private GameObject _LipSyncInterface;
    [SerializeField] private GameObject _Parameter;
 
    public void display()
    {
        _LipSyncInterface.SetActive(!_LipSyncInterface.activeSelf);
        _Parameter.SetActive(!_Parameter.activeSelf);
    }
    
    public void hide()
    {
        _LipSyncInterface.SetActive(!_LipSyncInterface.activeSelf);
        _Parameter.SetActive(!_Parameter.activeSelf);
    }
}
