namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using GSGD2.Utilities;

	/// <summary>
	/// Component class that handle anything related to the game object health. <see cref="TakeDamage(Damage)"/> will lower the health and call appropriates events, and <see cref="RestoreHealth(int)"/> will higher the health.
	/// It apply the settings of <see cref="Damage"/> with possible bump and <see cref="DamageOnTime"/> It has a <see cref="recoveryDuration"/> between TakeDamage().
	/// </summary>
	[SelectionBase]
	public class Damageable : MonoBehaviour
	{
		#region Fields
		[Header("References")]
		[SerializeField]
		private GameObjectDestroyer _gameObjectDestroyer = null;

		[Header("Mandatory reference for bump")]
		[SerializeField]
		new protected Rigidbody rigidbody = null;

		[SerializeField]
		protected DisplacementEstimationUpdater displacementEstimationUpdater = null;

		[Header("Balancing")]
		[SerializeField]
		protected int healthAtStart = 1;

		[SerializeField]
		protected int maxHealth = 1;

		[SerializeField]
		protected bool destroyIfHealthBelowZero = false;

		[Tooltip("Duration after taking damage where the damageable cannot take another damages")]
		[SerializeField]
		protected float recoveryDuration = 1f;

		[SerializeField]
		protected bool showDebug = false;
		[SerializeField]
		protected DamageOnTime _damageOnTime = null;

		protected int _currentHealth = 0;
		protected float _currentRecoveryDuration = 1f;
		#endregion Fields

		#region Properties
		public float CurrentHealth => _currentHealth;
		public float MaxHealth => maxHealth;
		public bool IsRecovering => _currentRecoveryDuration < recoveryDuration;
		public bool IsReceiveDOT => _damageOnTime.IsReceiveDOT;
		#endregion Properties

		#region Events
		public struct DamageableArgs
		{
			public int currentHealth;
			public int maxHealth;
			public Damage damage;
			public int healthRestored;

			public DamageableArgs(int currentHealth, int maxHealth, Damage damage, int healthRestored)
			{
				this.currentHealth = currentHealth;
				this.damage = damage;
				this.healthRestored = healthRestored;
				this.maxHealth = maxHealth;
			}

			public override string ToString()
			{
				return $"{currentHealth}/{maxHealth} from {damage.ToString()} | healthRestored :{healthRestored}";
			}
		}

		public delegate void DamageableEvent(Damageable sender, DamageableArgs args);
		public event DamageableEvent DamageTaken = null;
		public event DamageableEvent DamageTakenAndHealthBelowZero = null;
		public event DamageableEvent HealthRestored = null;

		public UnityEvent<Damageable, DamageableArgs> DamageTaken_UnityEvent = null;
		public UnityEvent<Damageable, DamageableArgs> HealthRestored_UnityEvent = null;
		public UnityEvent<Damageable, DamageableArgs> DamageTakenAndHealthBelowZero_UnityEvent = null;

		private DamageableArgs GetArgs(Damage damage, int healthRestored)
		{
			return new DamageableArgs(_currentHealth, maxHealth, damage, healthRestored);
		}
		#endregion Events

		#region Methods
		public void StartDamageOnTime(float rate, Damage damage)
		{
			if (enabled == false) return;
			damage.SetUpBump(displacementEstimationUpdater);
			_damageOnTime.StartDamageOnTime(rate, damage);
		}

		public void StopDamageOnTime() => _damageOnTime.StopDamageOnTime();

		public virtual bool TakeDamage(Damage damage)
		{
			if (CanTakeDamage(damage) == false) return false;

			//Debug.LogFormat("{0}.TakeDamage()", GetType());

			_currentHealth -= damage.DamageValue;
			_currentRecoveryDuration = 0;

			damage.TryApplyBump(rigidbody, displacementEstimationUpdater);

			var args = GetArgs(damage, 0);
			OnTakeDamage(damage);
			DamageTaken?.Invoke(this, args);
			if (_currentHealth <= 0)
			{
				OnTakeDamageAndHealthBelowZero(damage);
				DamageTakenAndHealthBelowZero?.Invoke(this, args);
				if (destroyIfHealthBelowZero == true)
				{
					if (_gameObjectDestroyer != null)
					{
						_gameObjectDestroyer.Destroy();
					}
					else
					{
						Debug.LogErrorFormat("{0}.TakeDamage() cannot destroy {1}, no gameObjectDestroyer found.", GetType().Name, name);
					}
				}
			}

			if (showDebug == true)
			{
				Debug.LogFormat("{0}.TakeDamage(Damage : {1})", GetType().Name, args.ToString());
			}
			return true;
		}

		public bool RestoreHealth(int healthPoint)
		{
			if (enabled == false || _currentHealth >= maxHealth) return false;
			_currentHealth = Mathf.RoundToInt(Mathf.Clamp(_currentHealth + healthPoint, 0f, maxHealth));
			HealthRestored?.Invoke(this, GetArgs(null, healthPoint));
			return true;
		}

		protected virtual bool CanTakeDamage(Damage damage)
		{
			bool result = enabled;
			result &= _currentRecoveryDuration >= recoveryDuration;
			result &= CheckIfWillDestroy() == false;
			result &= damage.DamageValue > 0;
			return result;
		}

		protected virtual void OnTakeDamage(Damage damage)
		{
		}

		protected virtual void OnTakeDamageAndHealthBelowZero(Damage damage)
		{
		}

		protected virtual void Awake() { }
		protected virtual void OnEnable()
		{
			_currentHealth = healthAtStart;
			_currentRecoveryDuration = recoveryDuration;
			DamageTaken -= OnDamageTaken;
			DamageTaken += OnDamageTaken;
			HealthRestored -= OnHealthRestored;
			HealthRestored += OnHealthRestored;
			DamageTakenAndHealthBelowZero -= OnDamageTakenAndHealthBelowZero;
			DamageTakenAndHealthBelowZero += OnDamageTakenAndHealthBelowZero;
		}

		protected virtual void OnDisable()
		{
			DamageTaken -= OnDamageTaken;
			HealthRestored -= OnHealthRestored;
		}

		protected virtual void Update()
		{
			_damageOnTime.UpdateDamageOnTimer(this);
			if (_currentRecoveryDuration < recoveryDuration)
			{
				_currentRecoveryDuration += Time.deltaTime;
			}
		}

		private bool CheckIfWillDestroy()
		{
			return _gameObjectDestroyer != null && _gameObjectDestroyer.WillDestroy == true;
		}

		#region Callbacks
		protected virtual void OnDamageTaken(Damageable sender, DamageableArgs args)
		{
			DamageTaken_UnityEvent.Invoke(sender, args);
		}

		protected virtual void OnHealthRestored(Damageable sender, DamageableArgs args)
		{
			HealthRestored_UnityEvent.Invoke(sender, args);
		}

		protected virtual void OnDamageTakenAndHealthBelowZero(Damageable sender, DamageableArgs args)
		{
			DamageTakenAndHealthBelowZero_UnityEvent.Invoke(sender, args);
		}
		#endregion Callbacks

		#endregion Methods
	}
}