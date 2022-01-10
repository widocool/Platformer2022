namespace GSGD2.Gameplay
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Player;
	using GSGD2.Utilities;

	/// <summary>
	/// Base component class for any Projectile that can be instantiated with the <see cref="ProjectileLauncherController"/>.
	/// Projectile can be redirected with <see cref="redirectProjectile"/> when entering trigger with <see cref="RedirectProjectileListener"/> component.
	/// On itself, do not move, it must be done by a child class.
	/// </summary>
	public abstract class AProjectile : MonoBehaviour
	{
		[Header("References")]
		[SerializeField]
		private GameObjectDestroyer _gameObjectDestroyer = null;

		[SerializeField]
		private PhysicsTriggerEvent _redirectProjectileTriggerEvent = null;

		[SerializeField]
		private PhysicsTriggerEvent _destroyOnCollisionTriggerEvent = null;

		[Header("Settings")]
		[SerializeField]
		private bool _destroyOnCollision = true;

		[SerializeField]
		protected float lifeSpan = 10f;

		[SerializeField]
		protected bool redirectProjectile = false;

		private InteractWithDamageable _interactWith = InteractWithDamageable.Everything;
		private DamageDealer _damageDealer = null;

		public void SetInteractWith(InteractWithDamageable interactWith)
		{
			_interactWith = interactWith;
		}

		protected virtual void Awake()
		{
			Destroy(gameObject, lifeSpan);
			_damageDealer = GetComponent<DamageDealer>();
		}

		protected virtual void OnEnable()
		{
			_redirectProjectileTriggerEvent._onTriggerEnter.RemoveListener(OnRedirectProjectileEnter);
			_redirectProjectileTriggerEvent._onTriggerEnter.AddListener(OnRedirectProjectileEnter);
			_destroyOnCollisionTriggerEvent._onTriggerEnter.RemoveListener(OnDestroyOnCollisionEnter);
			_destroyOnCollisionTriggerEvent._onTriggerEnter.AddListener(OnDestroyOnCollisionEnter);
		}

		protected virtual void OnDisable()
		{
			_redirectProjectileTriggerEvent._onTriggerEnter.RemoveListener(OnRedirectProjectileEnter);
			_destroyOnCollisionTriggerEvent._onTriggerEnter.RemoveListener(OnDestroyOnCollisionEnter);
		}

		protected virtual void OnRedirectProjectileEnter(PhysicsTriggerEvent triggerEvent, Collider other)
		{
			if (_gameObjectDestroyer.WillDestroy == true) return;

			if (redirectProjectile == true)
			{
				TryRedirectProjectile(other);
			}
		}

		protected virtual void OnDestroyOnCollisionEnter(PhysicsTriggerEvent triggerEvent, Collider other)
		{
			if (_gameObjectDestroyer.WillDestroy == true) return;
		
			// ignore instigator
			if (_damageDealer != null)
			{
				bool isInstigator = _damageDealer.IsInstigator(other.GetComponentInParent<IDamageInstigator>());
				if (isInstigator == true)
				{
					return;
				}
			}

			if (_destroyOnCollision == true)
			{
				if (CanDestroy(other.transform))
				{
					_gameObjectDestroyer.Destroy();
				}
			}
		}

		private void TryRedirectProjectile(Collider other)
		{
			var concreteOther = other.GetComponentInParent<RedirectProjectileListener>();
			if (concreteOther != null)
			{
				OnRedirected(concreteOther);
			}
		}

		protected virtual void OnRedirected(RedirectProjectileListener from)
		{
		}


		protected virtual bool CanDestroy(Transform other)
		{
			return _interactWith.CanInteractWith(other);
		}
	}
}