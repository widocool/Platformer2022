namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Player;

	//TODO AL : try add a way to buffer the damage. If a char end a dash in a spikes zone, the damage on time is not starting


	/// <summary>
	/// Class that works in conjunction with Damageable. It can be setup with a <see cref="Gameplay.PhysicsTriggerEvent"/>, apply
	/// </summary>
	[SelectionBase]
	public class DamageDealer : MonoBehaviour, IDamageInstigator
	{
		[SerializeField]
		private PhysicsTriggerEvent _physicsTriggerEvent = null;

		[SerializeField]
		private Damage _damage = null;

		[SerializeField]
		private bool _ignorePlayer = false;

		// TODO AL : move dot values to dot class
		[SerializeField]
		private bool _giveDOT = false;

		[SerializeField]
		private Damage _DOTDamage = null;

		[SerializeField]
		private float _DOTRate = 1f;

		private InteractWithDamageable _interactWith = InteractWithDamageable.Everything;
		private bool _hasGivenDamageThisFrame = false;

		private List<Damageable> _damageablesInRange = new List<Damageable>();
		public PhysicsTriggerEvent PhysicsTriggerEvent => _physicsTriggerEvent;

		public bool IsInstigator(IDamageInstigator instigator)
		{
			return (ReferenceEquals(instigator, _damage.Instigator)) == true;
		}

		public void SetInstigator(IDamageInstigator damageDealer)
		{
			_damage.SetInstigator(damageDealer);
			_DOTDamage.SetInstigator(damageDealer);
		}

		public void SetInteractWith(InteractWithDamageable interactWith)
		{
			_interactWith = interactWith;
		}

		public virtual void GiveDamage(Damageable damageable)
		{
			if (CanGiveDamage(damageable) == true)
			{
				bool hasGivenDamage = damageable.TakeDamage(_damage);
				if (hasGivenDamage == true)
				{
					_hasGivenDamageThisFrame = true;
					_damage.Bump.ResetCurrentForceCount();
				}
			}
		}

		protected virtual void OnEnable()
		{
			_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnTriggerEventEnter);
			_physicsTriggerEvent._onTriggerEnter.AddListener(OnTriggerEventEnter);

			_physicsTriggerEvent._onTriggerExit.RemoveListener(OnTriggerEventExit);
			_physicsTriggerEvent._onTriggerExit.AddListener(OnTriggerEventExit);

			_damage.SetInstigator(this);
			_DOTDamage.SetInstigator(this);
		}

		protected virtual void OnDisable()
		{
			_physicsTriggerEvent._onTriggerEnter.RemoveListener(OnTriggerEventEnter);
		}

		private bool CanGiveDamage(Damageable damageable)
		{
			bool canGiveDamage = _hasGivenDamageThisFrame == false;
			if (_ignorePlayer == true)
			{
				canGiveDamage = damageable.GetComponent<CubeController>() == null;
			}

			if (canGiveDamage == true)
			{
				canGiveDamage = _interactWith.CanInteractWith(damageable.transform);
			}
			return canGiveDamage;
		}

		private void OnTriggerEventEnter(PhysicsTriggerEvent physicsTriggerEvent, Collider other)
		{
			var concreteOther = other.GetComponentInParent<Damageable>();
			if (concreteOther != null && ReferenceEquals(concreteOther.gameObject, this.gameObject) == false && _damageablesInRange.Contains(concreteOther) == false)
			{
				if (CanGiveDamage(concreteOther) == false || IsInstigator(other.GetComponentInParent<IDamageInstigator>()) == true)
				{
					return;
				}

				_damageablesInRange.Add(concreteOther);
				GiveDamage(concreteOther);

				if (_giveDOT == true && concreteOther.IsReceiveDOT == true)
				{
					concreteOther.StartDamageOnTime(_DOTRate, _DOTDamage);
				}
			}
		}

		private void OnTriggerEventExit(PhysicsTriggerEvent physicsTriggerEvent, Collider other)
		{
			var concreteOther = other.GetComponentInParent<Damageable>();
			if (concreteOther != null && _damageablesInRange.Contains(concreteOther) == true)
			{
				if (_giveDOT == true && concreteOther.IsReceiveDOT == true)
				{
					concreteOther.StopDamageOnTime();
				}
				_damageablesInRange.Remove(concreteOther);
			}
		}

		private void Update()
		{
			if (_hasGivenDamageThisFrame == false)
			{
				for (int i = 0, length = _damageablesInRange.Count; i < length; i++)
				{
					GiveDamage(_damageablesInRange[i]);
				}
			}

			_hasGivenDamageThisFrame = false;
		}

		Transform IDamageInstigator.GetTransform() => transform;
	}
}