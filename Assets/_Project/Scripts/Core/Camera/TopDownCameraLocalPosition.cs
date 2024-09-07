using UnityEngine;

namespace Game.Components.Camera
{
    public class TopDownCameraLocalPosition
    {
        private Transform cameraLocalPositionParent_;
        private Vector3 localPositionVelocity_ = Vector3.zero;
        private Vector2 cameraPositionRectangle_;

        public TopDownCameraLocalPosition(Transform cameraLocalPositionParent)
        {
            cameraLocalPositionParent_ = cameraLocalPositionParent;
        }
        //Updates X , Z  coordinate of Local Camera position for better Road view
        public void SetLocalCameraPosition(Vector3 velocity, Vector2 screenAspectRatio, float smoothSpeed, float returnSmoothSpeed)
        {
            UpdateRectangle(velocity, screenAspectRatio, smoothSpeed);
            Vector3 targetPosition = new Vector3(cameraPositionRectangle_.x, cameraLocalPositionParent_.localPosition.y, cameraPositionRectangle_.y);
            cameraLocalPositionParent_.localPosition =
                Vector3.SmoothDamp(cameraLocalPositionParent_.localPosition, targetPosition, ref localPositionVelocity_, returnSmoothSpeed);
        }

        private void UpdateRectangle(Vector3 velocity, Vector2 screenAspectRatio, float smoothSpeed)
        {
            var direction = velocity.normalized;
            var speed = velocity.magnitude;

            cameraPositionRectangle_ = new Vector2(direction.x * speed * screenAspectRatio.x * smoothSpeed,
                             direction.z * speed * screenAspectRatio.y * smoothSpeed);
        }
    }
}