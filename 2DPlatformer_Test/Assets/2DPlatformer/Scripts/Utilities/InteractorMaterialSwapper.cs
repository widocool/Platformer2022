namespace GSGD2.Utilities
{
	using GSGD2.Gameplay;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Example of a class that listen to <see cref="AInteractor"/> event and change material on selected meshes. It is optionnal.
	/// </summary>
	public class InteractorMaterialSwapper : MonoBehaviour
	{
		[SerializeField]
		private AInteractor _interactor = null;

		[SerializeField]
		private MaterialDescription _materialDescription = null;

		[SerializeField]
		private MeshRenderer[] _meshRenderersToChange = null;

		private MaterialDescription _materialDescriptionInstance = null;

		private void Awake()
		{
			_materialDescriptionInstance = _materialDescription.CreateNew();
		} 

		private void OnEnable()
		{
			if (_meshRenderersToChange.Length > 0 && _materialDescription != null)
			{
				_materialDescriptionInstance.SetCachedOriginalMaterial(_meshRenderersToChange);
				_interactor.TriggerEntered -= InteractorOnTriggerEntered;
				_interactor.TriggerEntered += InteractorOnTriggerEntered;
				_interactor.TriggerExited -= InteractorOnTriggerExited;
				_interactor.TriggerExited += InteractorOnTriggerExited;
			}
			else
			{
				enabled = false;
			}
		}

		private void OnDisable()
		{
			_interactor.TriggerEntered -= InteractorOnTriggerEntered;
			_interactor.TriggerExited -= InteractorOnTriggerExited;
		}

		private void InteractorOnTriggerEntered(AInteractor sender, AInteractor.MovingPlatformInteractorEventArgs args)
		{
			_materialDescriptionInstance.ChangeMaterial(_meshRenderersToChange, true);
		}

		private void InteractorOnTriggerExited(AInteractor sender, AInteractor.MovingPlatformInteractorEventArgs args)
		{
			_materialDescriptionInstance.ChangeMaterial(_meshRenderersToChange, false);
		}
	}
}