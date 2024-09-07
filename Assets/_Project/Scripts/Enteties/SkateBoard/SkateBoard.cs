using Game.Core;
using Game.Core.Input;
using UnityEngine;

namespace Game.Entities.SkateBoard {
	public class SkateBoard : MonoBehaviour {
		[field: SerializeField] private InputReader inputReader_;
		[field: SerializeField] private Rigidbody rigidbody_;
		[Space]
		[Header("Movement Settings")]
		[field: SerializeField] private float movementSpeed_ = 3.0f;
		[field: SerializeField] private float acceleration_ = 0.8f;  // How fast the skateboard reaches full speed
		[field: SerializeField] private float deceleration_ = 10.0f;  // How fast the skateboard slows down
		[field: SerializeField] private float turnSpeed_ = 0.005f;  // How fast the skateboard turns

		[Space]
		[Header("Leaning Settings")]
		[field: SerializeField] private float leanAmount_ = 15.0f;  // Maximum lean angle in degrees
		[field: SerializeField] private float leanSpeed_ = 0.8f;  // How fast the skateboard leans

		private Vector3 movement_;
		private Vector3 currentVelocity_;
		private float currentLeanAngle_;
		private Quaternion targetRotation_;

		public void Start() {
			GameManager.Instance.OnUpdate += ProcessSkateBoard;
			inputReader_.EnableGameplayInputActions();
		}

		private void ProcessSkateBoard() {
			Vector3 inputDirection = new Vector3(inputReader_.Direction.x, 0f, inputReader_.Direction.y);

			movement_ = inputDirection.sqrMagnitude > 0.01f
									? Vector3.Lerp(movement_, inputDirection.normalized * movementSpeed_, acceleration_ * Time.deltaTime)
									: Vector3.Lerp(movement_, Vector3.zero, deceleration_ * Time.deltaTime);
		}

		private void FixedUpdate() {
			HandleMovement();
			HandleLeaning();
			HandleRotation();
		}

		private void HandleMovement() {
			currentVelocity_ = Vector3.Lerp(currentVelocity_, movement_, acceleration_);
			rigidbody_.velocity = new Vector3(currentVelocity_.x, rigidbody_.velocity.y, currentVelocity_.z);
		}

		private void HandleRotation() {
			if (currentVelocity_.sqrMagnitude >= 0.01f) {
				targetRotation_ = Quaternion.LookRotation(currentVelocity_);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation_, turnSpeed_);
			}
		}

		private void HandleLeaning() {

			float leanDirection = inputReader_.Direction.x; // X axis input for left/right movement
			
			if (Mathf.Abs(leanDirection) < 0.3f) return;

			float targetLeanAngle = -leanDirection * leanAmount_;

			currentLeanAngle_ = Mathf.Lerp(currentLeanAngle_, targetLeanAngle, leanSpeed_);
			transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, currentLeanAngle_);

		}
	}
}
