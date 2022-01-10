namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Player;

	/// <summary>
	/// Register the starting world position of the player and all <see cref="Checkpoint"/>s register to PlayerStart so the next time the Player enter in a <see cref="KillZone"/>, it will be reset to the last Checkpoint passed.
	/// </summary>
	public class PlayerStart : MonoBehaviour
	{
		public enum ResetMode
		{
			PlayerStart,
			LastCheckpointOrPlayerStart
		}

		[SerializeField]
		private ResetMode _resetMode = 0;

		[SerializeField]
		private ParticleSystem _playerResetParticle = null;

		private Vector3 _cachedStartPlayerPosition = Vector3.zero;

		private CubeController _player = null;
		private Rigidbody _playerRigidbody = null;

		private List<Checkpoint> _checkpoints = new List<Checkpoint>();

		private Checkpoint _lastCheckpointPassed = null;

		public struct PlayerStartEventArgs
		{
			public Vector3 playerPositionBeforeReset;
			public Vector3 playerPositionAfterReset;
			public PlayerStartEventArgs(Vector3 playerPositionBeforeReset, Vector3 playerPositionAfterReset)
			{
				this.playerPositionBeforeReset = playerPositionBeforeReset;
				this.playerPositionAfterReset = playerPositionAfterReset;
			}
		}

		public delegate void CheckpointResetEvent(PlayerStart sender, PlayerStartEventArgs args);
		public event CheckpointResetEvent BeforePlayerPositionReset = null;
		public event CheckpointResetEvent AfterPlayerPositionReset = null;

		private void OnEnable()
		{
			_player = LevelReferences.Instance.Player;
			_cachedStartPlayerPosition = _player.transform.position;
			_playerRigidbody = _player.Rigidbody;
		}

		/// <summary>
		/// Set player position to the next or the previous checkpoint.
		/// </summary>
		/// <param name="nextCheckpoint">true set to next checkpoint, false to previous checkpoint.</param>
		public void SetPlayerPositionToCheckpoint(bool nextCheckpoint)
		{
			if (_checkpoints.Count == 0)
			{
				Debug.LogErrorFormat("{0}.SetPlayerPositionToCheckpoint() No checkpoint found in scene, aborting.", GetType().Name);
				return;
			}

			int indexOfLastCheckpointPassed = _checkpoints.IndexOf(_lastCheckpointPassed);
			if (indexOfLastCheckpointPassed == -1)
			{
				indexOfLastCheckpointPassed = 0;
			}

			indexOfLastCheckpointPassed = Mathf.RoundToInt(Mathf.Repeat(indexOfLastCheckpointPassed + (nextCheckpoint == true ? 1 : -1), _checkpoints.Count));
			_lastCheckpointPassed = _checkpoints[indexOfLastCheckpointPassed];
			DoResetPlayerPosition(_lastCheckpointPassed.transform.position);
		}

		/// <summary>
		/// Add a <see cref="Checkpoint"/> to PlayerStart checkpoints list. It should be done at OnEnable, since the <see cref="CheatManager"/> used this list to travel between Checkpoint.
		/// The  list is sorted by <see cref="Checkpoint.Index"/>.
		/// </summary>
		/// <param name="checkpoint">The checkpoint to add to.</param>
		/// <returns>Checkpoint has been successfully added.</returns>
		public bool AddCheckpoint(Checkpoint checkpoint)
		{
			bool hasAddedANewCheckpoint = false;
			if (_checkpoints.Contains(checkpoint) == false)
			{
				int i = 0, index = checkpoint.Index;
				var count = _checkpoints.Count;
				while (i < count && _checkpoints[i].Index < index)
				{
					i++;
				}
				if (i < count)
				{
					_checkpoints.Insert(i, checkpoint);
				}
				else
				{
					_checkpoints.Add(checkpoint);
				}
				hasAddedANewCheckpoint = true;
			}
			return hasAddedANewCheckpoint;
		}

		public void UpdateLastCheckpoint(Checkpoint checkpoint)
		{
			_lastCheckpointPassed = checkpoint;
		}

		public void ResetPlayerPosition()
		{
			ResetPlayerPosition(_resetMode);
		}

		public void ResetPlayerPosition(ResetMode _resetMode)
		{
			Vector3 resetPosition = Vector3.zero;
			switch (_resetMode)
			{
				case ResetMode.PlayerStart:
				{
					resetPosition = _cachedStartPlayerPosition;
				}
				break;
				case ResetMode.LastCheckpointOrPlayerStart:
				{
					if (_lastCheckpointPassed != null)
					{
						resetPosition = _lastCheckpointPassed.transform.position;
					}
					else
					{
						resetPosition = _cachedStartPlayerPosition;
					}
				}
				break;
				default: break;
			}
			DoResetPlayerPosition(resetPosition);
		}

		private void DoResetPlayerPosition(Vector3 atPosition)
		{
			PlayerStartEventArgs args = new PlayerStartEventArgs(_player.transform.position, atPosition);
			BeforePlayerPositionReset?.Invoke(this, args);
			_playerRigidbody.velocity = Vector3.zero;
			_player.transform.position = atPosition;
			var instance = Instantiate(_playerResetParticle, atPosition, _playerResetParticle.transform.rotation);
			AfterPlayerPositionReset?.Invoke(this, args);
		}
	}
}