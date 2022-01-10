namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Conveniance class that can instantiate a particle with its GameObject transform position and rotation.
	/// </summary>
	public class ParticleInstancier : MonoBehaviour
	{
		[SerializeField]
		private ParticleSystem _particleSystem = null;

		// Simplified command for unity event
		public void Instantiate() => Instantiate(out _);

		public void Instantiate(out ParticleSystem particleSystem)
		{
			particleSystem = Instantiate(_particleSystem, transform.position, transform.rotation);
		}
	}
}