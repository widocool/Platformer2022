namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Component that disable any renderer when entering play. Useful for editing renderer only like invisible colliders or triggers.
	/// </summary>
	public class DisableRendererOnPlay : MonoBehaviour
	{
		[SerializeField]
		private Renderer _renderer = null;

		private void Awake()
		{
			_renderer.enabled = false;
		}
	}
}