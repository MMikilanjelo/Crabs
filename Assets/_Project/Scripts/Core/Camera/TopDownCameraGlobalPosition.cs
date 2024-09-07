using UnityEngine;

namespace Game.Components.Camera
{
    public class TopDownCameraGlobalPosition
    {
        private Transform cameraGlobalTransformParent_;
        private Transform playerCarTransform_;

        private Vector3 globalPositionVelocity_ = Vector3.zero;
        private Vector3 desiredCameraPosition_;

        public TopDownCameraGlobalPosition(Transform cameraGlobalTransformParent, Transform playerCarTransform)
        {
            cameraGlobalTransformParent_ = cameraGlobalTransformParent;
            playerCarTransform_ = playerCarTransform;
        }

        //Updates X , Z  coordinate of Global Camera position
        public void SetGlobalCameraPosition(Vector3 offsetPosition, Vector3 carVelocity, Vector2 screenAspectRatio, float smoothSpeed, float maxSmoothSpeed)
        {
            UpdateDesiredCameraPosition(offsetPosition, carVelocity, screenAspectRatio);

            float distance = Vector3.Distance(cameraGlobalTransformParent_.position, desiredCameraPosition_);
            float dynamicSmoothSpeed = Mathf.Lerp(smoothSpeed, maxSmoothSpeed, distance);
            cameraGlobalTransformParent_.position = Vector3.SmoothDamp(cameraGlobalTransformParent_.position, desiredCameraPosition_, ref globalPositionVelocity_, dynamicSmoothSpeed);
        }

        private void UpdateDesiredCameraPosition(Vector3 offsetPosition, Vector3 carVelocity, Vector2 screenAspectRatio)
        {

            desiredCameraPosition_ = new Vector3(
                playerCarTransform_.position.x + offsetPosition.x + (carVelocity.x / screenAspectRatio.x),
                cameraGlobalTransformParent_.position.y,
                playerCarTransform_.position.z + offsetPosition.z + (carVelocity.z / screenAspectRatio.y)
            );
        }
    }
}