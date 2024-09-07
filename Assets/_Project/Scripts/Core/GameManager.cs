using System;
using Game.Components.Camera;
using Game.Entities.SkateBoard;
using UnityEngine;
namespace Game.Core {
	public class GameManager : Singleton<GameManager> {
		[field: SerializeField] private SkateBoard skateBoard_;
		[field: SerializeField] private SmoothTopDownCameraMovement cameraController_;
		public event Action OnUpdate = delegate { };

		protected override void Awake() {
			base.Awake();
			cameraController_.SetSkateBoard(skateBoard_);
			cameraController_.SetTarget(skateBoard_.transform);
		}

		/// <summary>
		///Only subscribe for On Update Event never put you code into Update Method From other class and here
		/// </summary>
		public void Update() {
			OnUpdate?.Invoke();
		}
	}
}
