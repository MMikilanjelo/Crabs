using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static GameInput;

namespace Game.Core.Input {
	[CreateAssetMenu(fileName = "Input Reader", menuName = "Input/InputReader")]
	public class InputReader : ScriptableObject, IGameplayActions {

		public event Action<Vector2> Move = delegate { };
		public event Action<bool> Jump = delegate { };

		private GameInput gameInput_;
		public Vector3 Direction => gameInput_.Gameplay.Move.ReadValue<Vector2>();


		private void OnEnable() {
			if (gameInput_ == null) {
				gameInput_ = new GameInput();
				gameInput_.Gameplay.SetCallbacks(this);
			}
		}
		public void EnableGameplayInputActions() => gameInput_.Enable();

		public void OnMove(InputAction.CallbackContext context) {
			Move?.Invoke(context.ReadValue<Vector2>());
		}

		public void OnJump(InputAction.CallbackContext context) {
			
			switch (context.phase) {
				case InputActionPhase.Started:
					Jump.Invoke(true);
					break;
				case InputActionPhase.Canceled:
					Jump.Invoke(false);
					break;
			}
		}
	}
}
