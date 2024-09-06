using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Game.Core.Input.GameInput;

namespace Game.Core.Input {
	[CreateAssetMenu(fileName = "Input Reader", menuName = "Input/InputReader")]
	public class InputReader : ScriptableObject, IGameplayActions {

		public event Action<Vector2> Move = delegate { };

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
	}
}
