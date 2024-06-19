using System;
using _Scripts._Version_1._0.Controllers.RoomController;
using UnityEngine;
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

        private IncomingLiveStreamData _incomingLiveStreamData;
        private IncomingLiveStreamData _previousIncomingLiveStreamData;

        Pose _incomingPose;
        Pose _previousIncomingPose;
        Gaze _incomingGaze;
        Gaze _previousIncomingGaze;

        private void Awake()
        {
            LiveStreamingRoomController liveStreamingRoomController = new LiveStreamingRoomController(this);

        }

        private void Start()
        {
            _previousIncomingLiveStreamData = new IncomingLiveStreamData(Array.Empty<AU>());
            _previousIncomingPose = new Pose();
            _previousIncomingGaze = new Gaze();
        }

        public void OnActionUnitsReceived(IncomingLiveStreamData data)
        {
            Debug.Log("Action Units Received in room service , Data : " + data);
            _incomingLiveStreamData = data;
            if(_previousIncomingLiveStreamData._actionUnits.Length == 0)
                _previousIncomingLiveStreamData = data;
            
        }

        public void OnHeadPoseReceived(Pose data)
        {
            _incomingPose = data;
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
                    if (difference < 0.1f) continue;

                    int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(currentBlendShape.Key);
                    if (blendShapeIndex != -1)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, currentBlendShape.Value * 60f);
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

        private void Update()
        {
            if (_incomingLiveStreamData != null)
                ApplyBlendShapeValueToAvatar();
        }
    }
}