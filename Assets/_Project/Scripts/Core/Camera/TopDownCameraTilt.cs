using UnityEngine;

namespace Game.Components.Camera
{
    public class TopDownCameraTilt
    {
        private Transform cameraRollingParent_;
        public TopDownCameraTilt(Transform cameraRollingParent)
        {
            cameraRollingParent_ = cameraRollingParent;
        }
        public void SetTiltAngle(Vector3 tiltAngleToSet)
        {
            cameraRollingParent_.localRotation = Quaternion.Euler(tiltAngleToSet.x, tiltAngleToSet.y, tiltAngleToSet.z);
        }
    }
}
