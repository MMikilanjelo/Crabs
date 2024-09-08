using System;
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
		[field: SerializeField] private float turnSpeed_ = 5f;  // How fast the skateboard turns
		[field: SerializeField] private float maxSteerAngle_ = 45f;  // Maximum steering angle per frame (in degrees)
		[field: SerializeField] private float groundCheckRadius_ = 0.5f; // Check if object is grounded 
		[field: SerializeField] private float jumpForce_ = 10f;
		[field: SerializeField] private LayerMask groundLayer_;
		[field: SerializeField] private Transform groundCheck_;


		private bool isGrounded_;
		private Vector3 movement_;
		private Vector3 currentVelocity_;
		private Vector3 inputDirection => new Vector3(inputReader_.Direction.x, 0f, inputReader_.Direction.y);
		private void Start() {
			GameManager.Instance.OnUpdate += ProcessSkateBoard;
			inputReader_.EnableGameplayInputActions();
			inputReader_.Jump+=HandleJump;
		}
		public Vector3 GetVelocity() => rigidbody_.velocity;
		private void ProcessSkateBoard() {
			movement_ = inputDirection.sqrMagnitude > 0.01f
				? Vector3.Lerp(movement_, inputDirection.normalized * movementSpeed_, acceleration_ * Time.deltaTime)
				: Vector3.Lerp(movement_, Vector3.zero, deceleration_ * Time.deltaTime);
		}

		private void Steer(Vector3 inputDirection) {
			Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
			Vector3 forward = transform.forward;

			float angleToTarget = Vector3.Angle(forward, inputDirection);

			if (angleToTarget > maxSteerAngle_) {
				inputDirection = Vector3.Slerp(forward, inputDirection, maxSteerAngle_ / angleToTarget);
				targetRotation = Quaternion.LookRotation(inputDirection);
			}

			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed_ * Time.deltaTime);
		}

		private void FixedUpdate() {
			HandleMovement();
			
			if (inputDirection.sqrMagnitude > 0.01f) {
				Steer(inputDirection);
			}

		}

		private void HandleMovement() {
			currentVelocity_ = Vector3.Lerp(currentVelocity_, movement_, acceleration_);
			rigidbody_.velocity = new Vector3(currentVelocity_.x, rigidbody_.velocity.y, currentVelocity_.z);
		}
		private void Update() {
			isGrounded_ = Physics.CheckSphere(groundCheck_.position, groundCheckRadius_, groundLayer_);
		}

		private void HandleJump(bool isJumping) {
			if (isJumping && isGrounded_) {
				rigidbody_.velocity = new Vector3(rigidbody_.velocity.x, jumpForce_, rigidbody_.velocity.z);
			}
			else {
				Debug.Log("error");
			}
		}

		

	}
}