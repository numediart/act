using System.Collections.Generic;
using UnityEngine;
namespace _Scripts._Version_1._0.Services.RoomServices.WoZRoomService
{
    
    public struct AvatarHeadMoveData
    {
        public double x;
        public double y;
        public double z;
    }
    public class WoZRoomService:AbstractRoomServices
    {
     
        private void RegisterEvents()
        {
            throw new System.NotImplementedException();
        }
        
        public void OnAvatarHeadMove (AvatarHeadMoveData data)
        {
            AvatarHeadMoveData avatarHeadMoveData = data;
            MainManager.Instance.HeadPoseController.HeadPoseUpdateByValue(avatarHeadMoveData.x, avatarHeadMoveData.y, avatarHeadMoveData.z);
        }
        public void OnAvatarBlendshapeMove(Dictionary<string,double> data)
        {
            Dictionary<string,double> avatarBlendshapeData = data;
            MainManager.Instance.BlendShapesController.ChangeBlendShapesByDict(avatarBlendshapeData);
        }
        public void OnBlendshapesTransition(Dictionary<string,double> data)
        {
            Dictionary<string,double> avatarBlendshapeData = data;
            MainManager.Instance.BlendShapesController.ChangeBlendShapesByDict(avatarBlendshapeData);
        }

        public void OnAvatarPoseTransition(Vector3 data, float duration)
        {
            MainManager.Instance.HeadPoseController.MakeRotTransition(data, duration);
        }
        
        

        
    }
}