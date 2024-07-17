using System.Collections;
using System.Collections.Generic;
using uLipSync;
using UnityEngine;

public class DeviceSelection : MonoBehaviour
{
    public uLipSync.uLipSyncMicrophone mic;
    public TMPro.TMP_Dropdown dropdown;

    void Start()
    {
        var devices = MicUtil.GetDeviceList();
        dropdown.ClearOptions();
        dropdown.AddOptions(devices.ConvertAll(d => d.name));
        dropdown.value = mic.index;
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }
    
    void OnValueChanged(int value)
    {
        mic.index = value;
        mic.StartRecord();
    }
    
    void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnValueChanged);
    }
    
    void Update()
    {
        if (mic.index != dropdown.value)
        {
            dropdown.value = mic.index;
        }
    }
    
    void OnEnable()
    {
        if (mic.index != dropdown.value)
        {
            dropdown.value = mic.index;
        }
    }
    
    void OnDisable()
    {
        dropdown.value = 0;
    }
}
