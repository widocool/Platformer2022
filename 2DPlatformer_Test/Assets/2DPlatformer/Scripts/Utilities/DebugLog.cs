namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Test class that can be used to print a message from a <see cref="UnityEvent"/>.
	/// </summary>
	public class DebugLog : MonoBehaviour
	{
		public void Log(string message)
		{
			Debug.LogFormat("{0} : {1}", transform.name, message);
		}
	}

}