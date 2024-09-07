using UnityEngine;

namespace Game.Components.Camera
{
    public class TopDownCameraHeight
    {
        private Transform _cameraGlobalTransformParent;

        public TopDownCameraHeight(Transform cameraGlobalTransformParent)
        {
            _cameraGlobalTransformParent = cameraGlobalTransformParent;
        }

        public void SetCameraHeight(float YOffset, float maxOffsetY, float speed, float speedThreshold)
        {
            float dynamicOffsetY = GetDynamicOffsetY(YOffset, maxOffsetY, speed, speedThreshold);
            //Updates Y coordinate of Global Camera position
            _cameraGlobalTransformParent.position = new Vector3(
                _cameraGlobalTransformParent.position.x,
                dynamicOffsetY,
                _cameraGlobalTransformParent.position.z
            );
        }

        private float GetDynamicOffsetY(float YOffset, float maxOffsetY, float speed, float speedThreshold)
        {
            return Mathf.Lerp(YOffset, maxOffsetY, speed / speedThreshold);
        }
    }
}