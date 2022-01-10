namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Utilities;

	/// <summary>
	/// CheckpointParent listen to Checkpoint event and swap materials and selected meshes.
	/// The swapped materials can be the first one only (no multiple materials by meshes).
	/// It is not necessary for Checkpoint to works.
	/// </summary>
	[SelectionBase]
	public class CheckpointFeedbackHandler : MonoBehaviour
	{
		[SerializeField]
		private Checkpoint _checkpoint = null;

		[SerializeField]
		private MaterialDescription _materialDescriptionCheckpointAdded = null;

		[SerializeField]
		private MeshRenderer[] _meshRenderersCheckpointAdded = null;

		[SerializeField]
		private MaterialDescription _materialDescriptionOnEnterAndExit = null;

		[SerializeField]
		private MeshRenderer[] _meshRenderersToChangeMaterialOnEnterAndExit = null;

		private MaterialDescription _materialDescriptionCheckpointAddedInstance = null;
		private MaterialDescription _materialDescriptionOnEnterAndExitInstance = null;

		private void OnEnable()
		{
			_materialDescriptionCheckpointAddedInstance = _materialDescriptionCheckpointAdded.CreateNew();
			_materialDescriptionOnEnterAndExitInstance = _materialDescriptionOnEnterAndExit.CreateNew();

			_materialDescriptionCheckpointAddedInstance.SetCachedOriginalMaterial(_meshRenderersCheckpointAdded);
			_materialDescriptionOnEnterAndExitInstance.SetCachedOriginalMaterial(_meshRenderersToChangeMaterialOnEnterAndExit);

			_checkpoint._checkpointTriggered -= CheckpointOnCheckpointTriggered;
			_checkpoint._checkpointTriggered += CheckpointOnCheckpointTriggered;
		}

		private void OnDisable()
		{
			_checkpoint._checkpointTriggered -= CheckpointOnCheckpointTriggered;
		}

		private void CheckpointOnCheckpointTriggered(Checkpoint checkpoint, Checkpoint.EventType eventType)
		{
			switch (eventType)
			{
				case Checkpoint.EventType.FirstTimeReachedCheckpoint:
				{
					if (_meshRenderersCheckpointAdded.Length > 0 && _materialDescriptionCheckpointAddedInstance.IsSwapped == false)
					{
						_materialDescriptionCheckpointAddedInstance.SwapMaterial(_meshRenderersCheckpointAdded);
					}
					_materialDescriptionOnEnterAndExitInstance.ChangeMaterial(_meshRenderersToChangeMaterialOnEnterAndExit, true);
				}
				break;
				case Checkpoint.EventType.CheckpointPassed:
				{
					_materialDescriptionOnEnterAndExitInstance.ChangeMaterial(_meshRenderersToChangeMaterialOnEnterAndExit, true);
				}

				break;
				case Checkpoint.EventType.CheckpointExited:
				{
					_materialDescriptionOnEnterAndExitInstance.ChangeMaterial(_meshRenderersToChangeMaterialOnEnterAndExit, false);
				}
				break;
				default:
					break;
			}
		}
	}
}