using System.Collections.Generic;
using Game.Core;
using Game.Core.Input;
using UnityEngine;

namespace Game.Entities.Characters {
	public class Crab : MonoBehaviour {
		[field: SerializeField] private InputReader inputReader_;  
		[field: SerializeField] private Rigidbody rigidbody_; 
		[field: SerializeField] private float movementSpeed_ = 5f;  

		Vector3 movement_;

		public void Start() {
			GameManager.Instance.OnUpdate += ProcessCrabs;
			inputReader_.EnableGameplayInputActions();
		}

		private void ProcessCrabs() {
			movement_ = new Vector3(inputReader_.Direction.x, 0f, inputReader_.Direction.y);

			HandleMovement();
		}

		private void HandleMovement() {
			rigidbody_.velocity = new Vector3(movement_.x * movementSpeed_, rigidbody_.velocity.y, movement_.z * movementSpeed_);
		}
	}
}
