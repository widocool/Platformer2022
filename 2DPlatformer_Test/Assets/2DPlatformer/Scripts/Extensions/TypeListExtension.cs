namespace GSGD2.Gameplay
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Extension class that inject the possibility to look up a list of components on a given Transform. If the type lists share the same base class, it can be returned by the generic version.
	/// </summary>
	public static class TypeListExtension
	{
		// Usable only if types has the same base class
		public static bool CanInteractWithType<T>(this List<Type> types, Transform otherToLookUp, out T concreteOther) where T : MonoBehaviour
		{
			for (int i = 0; i < types.Count; i++)
			{
				var type = types[i];
				concreteOther = otherToLookUp.GetComponentInParent(type) as T;
				if (concreteOther != null)
				{
					return concreteOther != null;
				}
			}
			return concreteOther = null;
		}

		public static bool CanInteractWithType(this List<Type> types, Transform otherToLookUp)
		{
			for (int i = 0; i < types.Count; i++)
			{
				var type = types[i];
				if (otherToLookUp.GetComponentInParent(type) != null)
				{
					return true;
				}
			}
			return false;
		}
	}
}