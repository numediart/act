using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using _Scripts._Version_1._0;
using _Scripts._Version_1._0.Managers.Network.WebSocket;
using _Scripts._Version_1._0.Managers.Network.WebSocket.Enum;
using UnityEngine;
using NetMQ;
using Newtonsoft.Json;
using Tobii.Research;
using Tobii.Research.Unity;
using Pose = _Scripts._Version_1._0.Utils.Pose;
using Gaze = _Scripts._Version_1._0.Gaze;

public class NewNetworkManager : MonoBehaviour
{
    private WebsocketManager client;
    private bool isConnected = false;

    public GameObject liveStreamingDataPrefab;
    public IncomingLiveStreamData _incomingLiveStreamData;

    public Transform headJoint;
    public Transform leftEyeJoint;
    public Transform rightEyeJoint;
    public Transform headNeckJoint;

    public AudioSource audioSource;

    private IncomingLiveStreamData _previousIncomingLiveStreamData;

    private Pose _incomingPose;
    private Pose _previousIncomingPose;
    private Gaze _incomingGaze;
    private Gaze _previousIncomingGaze;

    private bool _firstTime = true;
    private bool _firstTimePose = true;
    private bool _firstTimeGaze = true;
    private string liveStreamingDataEvent;

    public struct RoomCreationData
    {
        public readonly string? RoomName;
        public readonly string? RoomOwner;
        public readonly string? Password;

        public RoomCreationData(string? roomName, string? roomOwner, string? password)
        {
            RoomName = roomName;
            RoomOwner = roomOwner;
            Password = password;
        }
    }
    void Start()
    {
    }



    private Vector2 CalculateGazeAngles(Vector3 gazeDirection)
    {
        float pitch = Mathf.Atan2(gazeDirection.y, gazeDirection.z) * Mathf.Rad2Deg;
        float yaw = Mathf.Atan2(gazeDirection.x, gazeDirection.z) * Mathf.Rad2Deg;
        return new Vector2(yaw, pitch);
    }

    private async void StartClient()
    {
        client = WebsocketManager.Instance;
        Debug.Log("Connecting to server...");
        await client.Connect("ws://localhost:9001");

        Debug.Log("Connected to server");
     
    }


    private void FixedUpdate()
    {
        if (audioSource.clip != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    void Update()
    {
        if (isConnected && Input.GetKeyDown(KeyCode.Escape))
        {
          //  var message = new MsgFormat(
             //   EnumHelper.GetStringAttributeValueFromEnum(EnumEvents.RequestLiveStreamingRoom), new RoomCreationData(roomOwner:" me", roomName:"room1", password:"1234"));
         //   client.Send(message.ToString());
        }



    }
    
    
    private void OnLiveStreamingDataEyeGaze(string data)
    {
        _incomingGaze = JsonConvert.DeserializeObject<Gaze>(data);
        if (_firstTimeGaze)
        {
            _previousIncomingGaze = _incomingGaze;
            _firstTimeGaze = false;
        }
    }

    struct AudioIncomingData
    {
        public byte[] buffer;
        public int bytesRecorded;
    }

    private void OnLiveStreamingAudioData(string data)
    {
        AudioIncomingData audioIncomingData = JsonConvert.DeserializeObject<AudioIncomingData>(data);
        byte[] buffer = audioIncomingData.buffer;
        int bytesRecorded = audioIncomingData.bytesRecorded;
        AudioClip receivedClip = AudioClip.Create("ReceivedAudio", bytesRecorded / 2, 1, 48000, false);
        receivedClip.SetData(ConvertByteToFloat(buffer), 0);

        // Play the received AudioClip
        audioSource.clip = receivedClip;
    }

    // Convert byte array to float array (assuming 16-bit PCM audio)
    private float[] ConvertByteToFloat(byte[] byteArray)
    {
        int floatArrayLength = byteArray.Length / 2;
        float[] floatArray = new float[floatArrayLength];
        for (int i = 0; i < floatArrayLength; i++)
        {
            floatArray[i] = BitConverter.ToInt16(byteArray, i * 2) / 32768f; // convert 16-bit PCM to float
        }

        return floatArray;
    }

    private float Map(float currentValue, float inMin, float inMax, float min, float max)
    {
        return (currentValue - inMin) * (max - min) / (inMax - inMin) + min;
    }

  private void ApplyEyeGaze()
    {
      /*  var data = eyeTracker.LatestGazeData;
        if (data == null) return;
      
        
        Vector3 leftGaze = new Vector3(data.Left.GazePointInUserCoordinates.y, data.Left.GazePointInUserCoordinates.x/2,
            data.Left.GazePointInUserCoordinates.z/2);
        
        Vector3 rightGaze = new Vector3(data.Right.GazePointInUserCoordinates.y, data.Right.GazePointInUserCoordinates.x/2,
            data.Right.GazePointInUserCoordinates.z/2);
        
        
        leftEyeJoint.localRotation = Quaternion.Euler(CalculateGazeAngles(leftGaze));
        
        rightEyeJoint.localRotation = Quaternion.Euler(CalculateGazeAngles(rightGaze));
        */
      
    }

  

    

    void OnApplicationQuit()
    {
        isConnected = false;
        if (client != null)
        {
            client.Close();
        }

        NetMQConfig.Cleanup();
    }
}