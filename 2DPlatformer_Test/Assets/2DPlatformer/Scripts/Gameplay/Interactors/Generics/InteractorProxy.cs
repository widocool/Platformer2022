namespace GSGD2.Gameplay
{
	using GSGD2.Utilities;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Interactor that act as proxy for other interactors. It can be used to trigger several other <see cref="AInteractor"/> children classes.
	/// </summary>
	public class InteractorProxy : AInteractor
	{
		[SerializeField]
		private List<AInteractor> _interactors = null;

		[Header("Editor / ReadOnly")]
		[SerializeField]
		private DrawLinkUtility _drawLinkToPlatform = null;

		public override void InteractFromTriggerEnter(PhysicsTriggerEvent sender, Collider other)
		{
			if (ShouldInteractWith<InteractorActivator>(other) == true)
			{
				base.InteractFromTriggerEnter(sender, other);

				for (int i = 0, length = _interactors.Count; i < length; i++)
				{
					AInteractor interactor = _interactors[i];
					if (interactor)
					{
						interactor.InteractFromTriggerEnter(sender, other);
					}
				}
			}
		}

		public override void InteractFromTriggerStay(PhysicsTriggerEvent sender, Collider other)
		{
			if (ShouldInteractWith<InteractorActivator>(other) == true)
			{
				base.InteractFromTriggerStay(sender, other);
				for (int i = 0, length = _interactors.Count; i < length; i++)
				{
					AInteractor interactor = _interactors[i];
					if (interactor)
					{
						interactor.InteractFromTriggerStay(sender, other);
					}
				}
			}
		}

		public override void InteractFromTriggerExit(PhysicsTriggerEvent sender, Collider other)
		{
			if (ShouldInteractWith<InteractorActivator>(other) == true)
			{
				base.InteractFromTriggerExit(sender, other);
				for (int i = 0, length = _interactors.Count; i < length; i++)
				{
					AInteractor interactor = _interactors[i];
					if (interactor)
					{
						interactor.InteractFromTriggerExit(sender, other);
					}
				}
			}
		}

		public override void Interact()
		{
			for (int i = 0, length = _interactors.Count; i < length; i++)
			{
				AInteractor interactor = _interactors[i];
				if (interactor)
				{
					interactor.Interact();
				}
			}
		}

#if UNITY_EDITOR
		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			for (int i = 0, length = _interactors.Count; i < length; i++)
			{
				AInteractor interactor = _interactors[i];
				if (interactor != null)
				{
					_drawLinkToPlatform.DrawLink(transform.position, interactor.transform.position);
				}
			}

		}
#endif //UNITY_EDITOR
	}
}