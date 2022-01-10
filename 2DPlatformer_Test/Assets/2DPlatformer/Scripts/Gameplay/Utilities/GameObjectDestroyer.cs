namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Component class that permit to delay destroying a GameObject to the next frame. Since multiple component works and react to event that can be unordered and call at the same frame, we used it to prevent component that destroy the object to nullify other components.
	/// </summary>
	public class GameObjectDestroyer : MonoBehaviour
	{
		private bool _willDestroy = false;

		public bool WillDestroy => _willDestroy;

		public void Destroy()
		{
			if (_willDestroy == false)
			{
				_willDestroy = true;
				StartCoroutine(WaitForDestroy());
			}
		}

		IEnumerator WaitForDestroy()
		{
			yield return null;
			Destroy(gameObject);
		}
	}
}