namespace GSGD2.Player
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.InputSystem;

	/// <summary>
	/// Conveniance class that permit to hold <see cref="InputActionMap"/> and to swap input schemes by drag n dropping Scriptable Object references.
	/// </summary>
	[CreateAssetMenu(menuName = "GameSup/InputActionMapWrapper", fileName = "InputActionMapWrapper")]
	public class InputActionMapWrapper : ScriptableObject
	{
		[SerializeField]
		private InputActionMap _actionMap;

		public InputActionMap ActionMap => _actionMap;


		public bool TryFindAction(string name, out InputAction inputAction, bool enable = false)
		{
			inputAction = _actionMap.FindAction(name);
			if (inputAction == null)
			{
				Debug.LogErrorFormat("{0}TryFindAction() Unable to find {1} input action.", GetType().Name, name);
				return false;
			}
			if (enable == true)
			{
				inputAction.Enable();
			}
			return true;
		}
	}
}