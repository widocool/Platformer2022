namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Player;

	/// <summary>
	/// Interactor that handle a <see cref="PickupCommand"/> to be applied when interacting with it. It can be used as all other interactors
	/// </summary>
	public class PickupInteractor : AInteractor, ICommandSender, IDamageInstigator
	{
		[SerializeField]
		private PickupCommand _pickupCommand = null;

		public override void InteractFromTriggerEnter(PhysicsTriggerEvent sender, Collider other)
		{
			base.InteractFromTriggerEnter(sender, other);

			if (other.GetComponentInParent<CubeController>() != null)
			{
				Interact();
			}
		}

		public override void Interact()
		{
			_pickupCommand.Apply(this);
		}

		GameObject ICommandSender.GetGameObject() => gameObject;

		Transform IDamageInstigator.GetTransform() => transform;
	}
}