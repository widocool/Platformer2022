namespace GSGD2.Gameplay
{
	using GSGD2.Player;
	using GSGD2.Utilities;
	using UnityEngine;

	/// <summary>
	/// Interface that can be implemented by user of Damage to tell the receiver who has given its damage. <see cref="Damage"/>, <seealso cref="Damageable"/>, <seealso cref="DamageDealer"/>.
	/// </summary>
	public interface IDamageInstigator
	{
		public Transform GetTransform();
	}

	/// <summary>
	/// Encapsulation of all informations related to a Damage done to a Damageable. It contains raw damage value, the bump informations and the instigator (source) of the Damage.
	/// It can be setup by the inspector or created with a new Damage() and settings values by the constructor.
	/// </summary>
	[System.Serializable]
	public class Damage
	{
		[SerializeField]
		private int _damageValue = 1;

		[SerializeField]
		private Jump _bump = null;

		private IDamageInstigator _instigator = null;

		public int DamageValue => _damageValue;
		public IDamageInstigator Instigator => _instigator;
		public Jump Bump => _bump;

		public Damage() { }

		public Damage(int damageValue, IDamageInstigator instigator)
		{
			_damageValue = damageValue;
			_instigator = instigator;
		}

		public override string ToString()
		{
			var instigatorName = _instigator != null ? _instigator.GetTransform().name : string.Empty;
			return $"[Damage : {_damageValue} from {instigatorName}";
		}

		public void SetUpBump(DisplacementEstimationUpdater displacementEstimationUpdater)
		{
			_bump.displacementEstimationUpdater = displacementEstimationUpdater;
		}

		public void SetInstigator(IDamageInstigator damageInstigator)
		{
			_instigator = damageInstigator;
		}

		public void CleanBump()
		{
			_bump.displacementEstimationUpdater = null;
		}

		public void TryApplyBump(Rigidbody rigidbody, DisplacementEstimationUpdater displacementEstimationUpdater)
		{
			if (_bump == null || rigidbody == null || displacementEstimationUpdater == null) return;

			_bump.displacementEstimationUpdater = displacementEstimationUpdater;
			_bump.TryApplyForce(rigidbody);
			_bump.displacementEstimationUpdater = null;
		}
	}
}