namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	public class KillzoneEventDispatcher : MonoBehaviour
	{
		private PlayerStart _playerStart = null;

		[SerializeField]
		private UnityEvent _beforePlayerPositionReset = null;

		[SerializeField]
		private UnityEvent _afterPlayerPositionReset = null;

		private void OnEnable()
		{
			_playerStart = LevelReferences.Instance.PlayerStart;
			_playerStart.BeforePlayerPositionReset -= PlayerStartOnBeforePlayerPositionReset;
			_playerStart.BeforePlayerPositionReset += PlayerStartOnBeforePlayerPositionReset;

			_playerStart.AfterPlayerPositionReset -= PlayerStartOnAfterPlayerPositionReset;
			_playerStart.AfterPlayerPositionReset += PlayerStartOnAfterPlayerPositionReset;
		}


		private void OnDisable()
		{
			_playerStart.BeforePlayerPositionReset -= PlayerStartOnBeforePlayerPositionReset;
			_playerStart.AfterPlayerPositionReset -= PlayerStartOnAfterPlayerPositionReset;
		}

		private void PlayerStartOnBeforePlayerPositionReset(PlayerStart sender, PlayerStart.PlayerStartEventArgs args)
		{
			_beforePlayerPositionReset.Invoke();
		}

		private void PlayerStartOnAfterPlayerPositionReset(PlayerStart sender, PlayerStart.PlayerStartEventArgs args)
		{
			_afterPlayerPositionReset.Invoke();
		}
	}
}