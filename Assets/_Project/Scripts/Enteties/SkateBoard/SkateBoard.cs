using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Input;
using Game.Core.Utilities;
using UnityEngine;

namespace Game.Entities.SkateBoard
{
	public class SkateBoard : MonoBehaviour
	{
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
		[field: SerializeField] private LayerMask groundLayer_;
		[field: SerializeField] private Transform groundCheck_;

		[Space]
		[Header("Jump Settings")]
		[SerializeField] float jumpForce_ = 10f;
		[SerializeField] float jumpDuration_ = 0.5f;
		[SerializeField] float jumpCooldown_ = 0f;
		[SerializeField] float gravityMultiplier_ = 3f;

		private CountdownTimer jumpTimer_;
		private CountdownTimer jumpCooldownTimer_;
		private List<CountdownTimer> timers_;
		private float jumpVelocity_;
		private bool IsGrounded_;

		private Vector3 movement_;
		private Vector3 currentVelocity_;

		private Vector3 inputDirection => new Vector3(inputReader_.Direction.x, 0f, inputReader_.Direction.y);
		public Vector3 GetVelocity() => rigidbody_.velocity;

		private const float ZeroF = 0.0f;

		private void Awake()
		{
			GameManager.Instance.OnUpdate += ProcessSkateBoard;
			inputReader_.EnableGameplayInputActions();
			SetupTimers();
		}
		void OnEnable()
		{
			inputReader_.Jump += (bool performed) =>
			{
				OnJump(performed);
				HandleJump();
			};
		}
		void OnDisable()
		{
			inputReader_.Jump -= (bool performed) =>
			{
				OnJump(performed);
				HandleJump();
			};
		}
		private void FixedUpdate()
		{
			HandleMovement();
			if (inputDirection.sqrMagnitude > 0.01f)
				Steer(inputDirection);
		}

		private void SetupTimers()
		{
			jumpTimer_ = new CountdownTimer(jumpDuration_);
			jumpCooldownTimer_ = new CountdownTimer(jumpCooldown_);

			jumpTimer_.OnTimerStart += () => jumpVelocity_ = jumpForce_;
			jumpTimer_.OnTimerStop += () => jumpCooldownTimer_.Start();

			timers_ = new List<CountdownTimer> { jumpCooldownTimer_, jumpTimer_ };
		}
		private void ProcessSkateBoard()
		{
			movement_ = inputDirection.sqrMagnitude > 0.01f
				? Vector3.Lerp(movement_, inputDirection.normalized * movementSpeed_, acceleration_ * Time.deltaTime)
				: Vector3.Lerp(movement_, Vector3.zero, deceleration_ * Time.deltaTime);

			IsGrounded_ = Physics.CheckSphere(groundCheck_.position, groundCheckRadius_, groundLayer_);

			HandleTimers();
		}

		private void Steer(Vector3 inputDirection)
		{
			Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
			Vector3 forward = transform.forward;

			float angleToTarget = Vector3.Angle(forward, inputDirection);

			if (angleToTarget > maxSteerAngle_)
			{
				inputDirection = Vector3.Slerp(forward, inputDirection, maxSteerAngle_ / angleToTarget);
				targetRotation = Quaternion.LookRotation(inputDirection);
			}

			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed_ * Time.deltaTime);
		}


		private void HandleTimers()
		{
			foreach (var timer in timers_)
			{
				timer.Tick(Time.deltaTime);
			}
		}
		private void HandleMovement()
		{
			currentVelocity_ = Vector3.Lerp(currentVelocity_, movement_, acceleration_);
			rigidbody_.velocity = new Vector3(currentVelocity_.x, rigidbody_.velocity.y, currentVelocity_.z);
		}

		public void HandleJump()
		{
			if (!jumpTimer_.IsRunning && IsGrounded_)
			{
				jumpVelocity_ = ZeroF;
				return;
			}

			if (!jumpTimer_.IsRunning)
				jumpVelocity_ += Physics.gravity.y * gravityMultiplier_ * Time.fixedDeltaTime;

			rigidbody_.velocity = new Vector3(rigidbody_.velocity.x, jumpVelocity_, rigidbody_.velocity.z);
		}
		private void OnJump(bool performed)
		{
			if (performed && !jumpTimer_.IsRunning && !jumpCooldownTimer_.IsRunning && IsGrounded_)
				jumpTimer_.Start();

			else if (!performed && jumpTimer_.IsRunning)
				jumpTimer_.Stop();
		}
	}
}