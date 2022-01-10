namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Player;
	using GSGD2.Extensions;

	/// <summary>
	/// Base class for non-physics driven projectile. Move forward at given <see cref="_speed"/>.
	/// It handle redirection from <see cref="RedirectProjectileListener"/>.
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	public class Projectile : AProjectile
	{
		[SerializeField]
		private float _speed = 1f;

		protected override void Awake()
		{
			base.Awake();
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.SetUpPassiveRigidbody(CollisionDetectionMode.ContinuousSpeculative);
		}

		protected override void OnRedirected(RedirectProjectileListener from)
		{
			base.OnRedirected(from);
			transform.position = from.Origin.position;
			transform.rotation = Quaternion.LookRotation(from.GetDirection());
		}

		private void Update()
		{
			transform.position += Time.deltaTime * _speed * transform.forward;
		}
	}
}