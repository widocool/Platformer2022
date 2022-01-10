namespace GSGD2.Gameplay
{
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Generic Command. The Command Pattern is essentially encapsulating a behavior into an object that can be passed around. Here, we use ScriptableObject serialization and dragndrop reference capacity to pass our behavior.
	/// </summary>
	public abstract class ACommand : ScriptableObject
	{
		/// <summary>
		/// Method to call when using the Command
		/// </summary>
		/// <param name="from"></param>
		public abstract void Apply(ICommandSender from);
	}
}