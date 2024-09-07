using UnityEngine;

namespace Game.Components.Camera
{
    public class TopDownCameraGlobalPosition
    {
        private Transform _cameraGlobalTransformParent;
        private Transform _playerCarTransform;

        private Vector3 _globalPositionVelocity = Vector3.zero;
        private Vector3 _desiredCameraPosition;

        public TopDownCameraGlobalPosition(Transform cameraGlobalTransformParent, Transform playerCarTransform)
        {
            _cameraGlobalTransformParent = cameraGlobalTransformParent;
            _playerCarTransform = playerCarTransform;
        }

        //Updates X , Z  coordinate of Global Camera position
        public void SetGlobalCameraPosition(Vector3 offsetPosition, Vector3 carVelocity, Vector2 screenAspectRatio, float smoothSpeed, float maxSmoothSpeed)
        {
            UpdateDesiredCameraPosition(offsetPosition, carVelocity, screenAspectRatio);

            float distance = Vector3.Distance(_cameraGlobalTransformParent.position, _desiredCameraPosition);
            float dynamicSmoothSpeed = Mathf.Lerp(smoothSpeed, maxSmoothSpeed, distance);
            _cameraGlobalTransformParent.position = Vector3.SmoothDamp(_cameraGlobalTransformParent.position, _desiredCameraPosition, ref _globalPositionVelocity, dynamicSmoothSpeed);
        }

        private void UpdateDesiredCameraPosition(Vector3 offsetPosition, Vector3 carVelocity, Vector2 screenAspectRatio)
        {

            _desiredCameraPosition = new Vector3(
                _playerCarTransform.position.x + offsetPosition.x + (carVelocity.x / screenAspectRatio.x),
                _cameraGlobalTransformParent.position.y,
                _playerCarTransform.position.z + offsetPosition.z + (carVelocity.z / screenAspectRatio.y)
            );
        }
    }
}