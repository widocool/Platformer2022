namespace GSGD2.Gameplay
{
	using UnityEngine;

	/// <summary>
	/// Generic interface to be implemented by the caller of a <see cref="Command"/> .
	/// </summary>
	public interface ICommandSender
	{
		GameObject GetGameObject();
	}
}