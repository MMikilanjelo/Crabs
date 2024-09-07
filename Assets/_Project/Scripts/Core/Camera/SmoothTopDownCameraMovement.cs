using Game.Entities.SkateBoard;
using UnityEngine;

namespace Game.Components.Camera {
	public class SmoothTopDownCameraMovement : MonoBehaviour {
		[Header("Transform references")]
		[SerializeField] private Transform cameraGlobalPositionParent_;
		[SerializeField] private Transform cameraLocalPositionParent_;
		[SerializeField] private Transform cameraRollingParent_;


		[Space]

		[Header("Movement Settings")]
		[Tooltip("Offset position from the targetTransfor_")]
		[SerializeField] private Vector3 offset_ = new Vector3(0, -1, -4);

		[Tooltip("Smoothing speed for camera movement")]
		[SerializeField] private float smoothSpeed_ = 0.015f;
		[Tooltip("Max Smoothing speed for camera movement")]
		[SerializeField] private float maxSmoothSpeed_ = 0.02f;
		[Tooltip("Smoothing speed for camera returning back to player")]
		[SerializeField] private float returnSmoothSpeed_ = 0.6f;

		[Tooltip("Initial tilt angle for top-down view")]
		[SerializeField] private Vector3 initialTiltAngle_ = new Vector3(25, 0, 0);

		[Tooltip("Maximum Y offset for dynamic adjustment")]
		[SerializeField] private float maxOffsetY_ = 4.0f;

		[Tooltip("Speed threshold for maximum Y-offset")]
		[SerializeField] private float speedThreshold_ = 	40.0f;

		[Space]
		[Header("Camera Reference")]
		[SerializeField] private UnityEngine.Camera mainCamera_;

		private Transform targetTransform_;

		private SkateBoard skateBoard_;

		private Vector3 _targetVelocity => skateBoard_?.GetVelocity() ?? Vector3.zero;
		private float _targetSpeed => skateBoard_?.GetVelocity().magnitude ?? 0;

		private Vector2Int screenAspectRatio_;

		private TopDownCameraTilt topDownCameraTilt_;
		private TopDownCameraHeight topDownCameraHeight_;
		private TopDownCameraLocalPosition topDownCameraLocalPosition_;
		private TopDownCameraGlobalPosition topDownCameraGlobalPosition_;


		private void Start() {
			screenAspectRatio_ = new Vector2Int(16, 9);
			topDownCameraHeight_ = new TopDownCameraHeight(cameraGlobalPositionParent_);
		 topDownCameraTilt_ = new TopDownCameraTilt(cameraRollingParent_);

			topDownCameraLocalPosition_ = new TopDownCameraLocalPosition(cameraLocalPositionParent_);
			topDownCameraGlobalPosition_ = new TopDownCameraGlobalPosition(cameraGlobalPositionParent_, targetTransform_);


		 topDownCameraTilt_.SetTiltAngle(initialTiltAngle_);
		}

		private void LateUpdate() {
			if (targetTransform_ == null) return;

			UpdatePosition();
		}

		public void SetTarget(Transform targetToSet) => targetTransform_ = targetToSet;
		public void SetSkateBoard(SkateBoard skateBoardToSet) => skateBoard_ = skateBoardToSet;

		private void UpdatePosition() {
			topDownCameraHeight_.SetCameraHeight(offset_.y, maxOffsetY_, _targetSpeed, speedThreshold_);
			// Update camera rectangle for better road view
			topDownCameraLocalPosition_.SetLocalCameraPosition(_targetVelocity, screenAspectRatio_, smoothSpeed_, returnSmoothSpeed_);
			//Update Camera Global Position
			topDownCameraGlobalPosition_.SetGlobalCameraPosition(offset_, _targetVelocity, screenAspectRatio_, smoothSpeed_, maxSmoothSpeed_);
		}
	}
}