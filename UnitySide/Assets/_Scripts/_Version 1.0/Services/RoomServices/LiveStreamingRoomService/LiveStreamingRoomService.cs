using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using _Scripts._Version_1._0.Controllers.RoomController;
using _Scripts._Version_1._0.Utils;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Pose = _Scripts._Version_1._0.Utils.Pose;

namespace _Scripts._Version_1._0.Services.RoomServices.LiveStreamingRoomService
{
    public class LiveStreamingRoomService : AbstractRoomServices
    {
        [SerializeField] private GameObject liveStreamingDataPrefab;
        [SerializeField] private Transform headJoint;
        [SerializeField] private Transform headNeckJoint;

        [SerializeField] private Transform leftEyeJoint;
        [SerializeField] private Transform rightEyeJoint;

        [SerializeField] private Button recordingButton;

        private IncomingLiveStreamData _incomingLiveStreamData;
        private IncomingLiveStreamData _previousIncomingLiveStreamData;

        private List<IncomingLiveStreamData> _incomingLiveStreamDataList = new List<IncomingLiveStreamData>();
        private List<Pose> _incomingPoseList = new List<Pose>();


        private List<BlendShapeList> _incomingMediaPipeBlendshapeDataList = new List<BlendShapeList>();
        private List<BlendShapeList> _previousIncomingMediaPipeBlendshapeDataList = new List<BlendShapeList>();

        Pose _incomingPose;
        Pose _previousIncomingPose;
        Gaze _incomingGaze;
        Gaze _previousIncomingGaze;

        private bool _isRecording;
        double _previousTime;
        
        private AudioSource audioSource;

        private void Awake()
        {
            LiveStreamingRoomController liveStreamingRoomController = new LiveStreamingRoomController(this);
            audioSource = GameObject.Find("LipSync").GetComponent<AudioSource>();
            // audio source has to be attached to lip sync 
            
        }

        private void Start()
        {
            _previousIncomingLiveStreamData = new IncomingLiveStreamData(Array.Empty<AU>(), 0, 0);
            _previousIncomingPose = new Pose();
            _previousIncomingGaze = new Gaze();
        }

        public void OnActionUnitsReceived(IncomingLiveStreamData data)
        {
            _incomingLiveStreamData = data;
            if (_previousIncomingLiveStreamData._actionUnits.Length == 0)
                _previousIncomingLiveStreamData = data;
            if (_isRecording)
                _incomingLiveStreamDataList.Add(data);
        }

        public void OnHeadPoseReceived(Pose data)
        {
            _incomingPose = data;
            _previousIncomingPose ??= data;
            if (_isRecording)
                _incomingPoseList.Add(data);
        }

        public void OnGazeReceived(Gaze data)
        {
            _incomingGaze = data;
        }

        private void ApplyBlendShapeValueToAvatar()
        {
            if (!liveStreamingDataPrefab.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
            {
                Debug.LogError("SkinnedMeshRenderer not found on the liveStreamingDataPrefab.");
                return;
            }

            for (int i = 0; i < _incomingLiveStreamData._actionUnits.Length; i++)
            {
                var actionUnit = _incomingLiveStreamData._actionUnits[i];
                var previousActionUnit = _previousIncomingLiveStreamData._actionUnits[i];

                foreach (var currentBlendShape in actionUnit.BlendShapeList)
                {
                    float difference =
                        Mathf.Abs(previousActionUnit
                            .BlendShapeList[actionUnit.BlendShapeList.IndexOf(currentBlendShape)]
                            .Value - currentBlendShape.Value);
                    if (difference < 0.1f)
                    {
                        continue;
                    }

                    ;

                    int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(currentBlendShape.Key);
                    if (blendShapeIndex != -1)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, currentBlendShape.Value * 55f);
                        previousActionUnit.BlendShapeList[actionUnit.BlendShapeList.IndexOf(currentBlendShape)].Value =
                            currentBlendShape.Value;
                    }
                    else
                    {
                        Debug.LogWarning("Blendshape not found: " + currentBlendShape.Key);
                    }
                }
            }
        }

        private void ApplyHeadPoseToAvatar()
        {
            if (_incomingPose == null) return;

            if (Mathf.Abs(_incomingPose.pose_Rx - _previousIncomingPose.pose_Rx) < 0.01f &&
                Mathf.Abs(_incomingPose.pose_Ry - _previousIncomingPose.pose_Ry) < 0.01f &&
                Mathf.Abs(_incomingPose.pose_Rz - _previousIncomingPose.pose_Rz) < 0.01f)
            {
                return;
            }

            Vector3 vectorRotation = new Vector3(_incomingPose.pose_Rx * Mathf.Rad2Deg,
                _incomingPose.pose_Ry * Mathf.Rad2Deg, _incomingPose.pose_Rz * Mathf.Rad2Deg);
            float headMultiplier = 0.65f;
            float neckMultiplier = 0.35f;
            Quaternion headRotation = Quaternion.Euler((vectorRotation * headMultiplier) + new Vector3(-19.051f, 0, 0));
            headJoint.localRotation = headRotation;
            headNeckJoint.localRotation =
                Quaternion.Euler((vectorRotation * neckMultiplier) + new Vector3(26.705f, 0, 0));
            _previousIncomingPose = _incomingPose;
        }

        private void FixedUpdate()
        {
            if (audioSource.clip != null && !audioSource.isPlaying)
                audioSource.Play();
        }

        private void Update()
        {
            if (_incomingLiveStreamData != null)
                ApplyBlendShapeValueToAvatar();

            if (_incomingPose != null)
                ApplyHeadPoseToAvatar();

            if (_incomingMediaPipeBlendshapeDataList.Count > 0)
                ApplyMediapipeDataToAvatar();
        }

        public void OnMediaPipeBlendshapeData(List<BlendShapeList> data)
        {
            _incomingMediaPipeBlendshapeDataList = data;
            if (_previousIncomingMediaPipeBlendshapeDataList.Count == 0)
                _previousIncomingMediaPipeBlendshapeDataList = data;
        }

        private void ApplyMediapipeDataToAvatar()
        {
            if (!liveStreamingDataPrefab.TryGetComponent(out SkinnedMeshRenderer skinnedMeshRenderer))
            {
                Debug.LogError("SkinnedMeshRenderer not found on the liveStreamingDataPrefab.");
                return;
            }

            Mesh mesh = skinnedMeshRenderer.sharedMesh;
            Dictionary<string, int> blendShapeIndexCache = new Dictionary<string, int>();

            foreach (var blendShapeData in _incomingMediaPipeBlendshapeDataList)
            {
                string blendShapeName = blendShapeData.Key;
                float newBlendShapeValue = blendShapeData.Value;

                int blendShapeIndex;
                if (!blendShapeIndexCache.TryGetValue(blendShapeName, out blendShapeIndex))
                {
                    blendShapeIndex = mesh.GetBlendShapeIndex(blendShapeName);
                    blendShapeIndexCache[blendShapeName] = blendShapeIndex;
                }

                if (blendShapeIndex == -1)
                {
                    Debug.LogWarning("Blendshape not found: " + blendShapeName);
                    continue;
                }

                int dataIndex = _incomingMediaPipeBlendshapeDataList.IndexOf(blendShapeData);
                var previousBlendShapeData = _previousIncomingMediaPipeBlendshapeDataList[dataIndex];

                float previousBlendShapeValue = previousBlendShapeData.Value;
                float difference = Mathf.Abs(previousBlendShapeValue - newBlendShapeValue);

                if (difference >= 0.1f)
                {
                    skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, newBlendShapeValue * 55f);
                    previousBlendShapeData.Value = newBlendShapeValue;
                }
            }
        }
        
        struct AudioIncomingData
        {
            public long timestamp;
            public byte[] buffer;
            public int bytesRecorded;
            public string roomId;
        }

        
        public void OnLiveStreamingAudioData(string data)
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
    


        public struct RecordData
        {
            public List<IncomingLiveStreamData> incomingLiveStreamData;
            public List<Pose> incomingPose;
            public int TargetFps;

            public RecordData(List<IncomingLiveStreamData> incomingLiveStreamData, List<Pose> incomingPose,
                int targetFps = 30)
            {
                this.incomingLiveStreamData = incomingLiveStreamData;
                this.incomingPose = incomingPose;
                TargetFps = targetFps;
            }

            public object ToJson()
            {
                return new
                {
                    incomingLiveStreamData,
                    incomingPose
                };
            }
        }

        public void OnRecordButtonClicked()
        {
            Debug.Log("Record Button Clicked");
            _isRecording = true;
            _incomingLiveStreamDataList.Clear();
            _incomingPoseList.Clear();
            recordingButton.onClick.RemoveAllListeners();
            recordingButton.onClick.AddListener(OnStopButtonClicked);
            _previousTime = Time.time;
            //StartCoroutine(StopRecording(10.0f));
        }

        /// <summary>
        /// Function to have fixed record time and automtic end of recording expression
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator StopRecording(float time)
        {
            yield return new WaitForSeconds(time);
            if (_isRecording)
                OnStopButtonClicked();
        }

        public void OnStopButtonClicked()
        {
            Debug.Log("Stop Button Clicked");
            // stop recording and save the JSON file
            double endTime = Time.time;
            int incomingLiveStreamDataCount = _incomingLiveStreamDataList.Count;

            RecordData recordData = new RecordData(_incomingLiveStreamDataList, _incomingPoseList);
            string json = JsonConvert.SerializeObject(recordData.ToJson());
            // open file dialog to choose the path and the name of the file

            //start coroutine to save the file
            FileDialog fs = gameObject.AddComponent<FileDialog>();
            string path = fs.SaveFileDialog("Save File", "intensity", "json");
            System.IO.File.WriteAllText(path, json);
            _isRecording = false;
            recordingButton.onClick.RemoveAllListeners();
            recordingButton.onClick.AddListener(OnRecordButtonClicked);
        }
    }
}