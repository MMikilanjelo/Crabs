using System;

namespace Game.Core {
	public class GameManager : Singleton<GameManager> {

		public event Action OnUpdate = delegate { };

		protected override void Awake() {
			base.Awake();
		}

		/// <summary>
		///Only subscribe for On Update Event never put you code into Update Method From other class and here
		/// </summary>
		public void Update() {
			OnUpdate?.Invoke();
		}
	}
}
