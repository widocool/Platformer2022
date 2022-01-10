namespace GSGD2.Gameplay
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Base component class for any object that needs to be reset when entering a <see cref="Killzone"/>
	/// </summary>
	public abstract class AKillZoneReceiver : MonoBehaviour
	{
		public abstract void OnEnterKillzone(Killzone killzone);
	}
}