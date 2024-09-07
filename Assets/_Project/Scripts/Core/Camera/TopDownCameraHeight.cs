using UnityEngine;

namespace Game.Components.Camera
{
    public class TopDownCameraHeight
    {
        private Transform cameraGlobalTransformParent_;

        public TopDownCameraHeight(Transform cameraGlobalTransformParent)
        {
           cameraGlobalTransformParent_ = cameraGlobalTransformParent;
        }

        public void SetCameraHeight(float YOffset, float maxOffsetY, float speed, float speedThreshold)
        {
            float dynamicOffsetY = GetDynamicOffsetY(YOffset, maxOffsetY, speed, speedThreshold);
            //Updates Y coordinate of Global Camera position
           cameraGlobalTransformParent_.position = new Vector3(
               cameraGlobalTransformParent_.position.x,
                dynamicOffsetY,
               cameraGlobalTransformParent_.position.z
            );
        }

        private float GetDynamicOffsetY(float YOffset, float maxOffsetY, float speed, float speedThreshold)
        {
            return Mathf.Lerp(YOffset, maxOffsetY, speed / speedThreshold);
        }
    }
}