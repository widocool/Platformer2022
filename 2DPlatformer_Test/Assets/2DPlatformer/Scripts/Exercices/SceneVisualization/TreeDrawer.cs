#if UNITY_EDITOR
namespace GSGD2.Editor
{
	using GSGD2.Utilities;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	//[ExecuteInEditMode]
	public class TreeDrawer : MonoBehaviour
	{
		[SerializeField]
		private float _thickness = 3f;

		private void OnDrawGizmos()
		{
			DrawTree(transform);
		}

		private void DrawTree(Transform parent)
		{
			for (int i = 0, childCount = parent.childCount; i < childCount; i++)
			{
				var child = parent.GetChild(i);
				Handles.DrawLine(parent.position, child.position, _thickness);

				if (child.childCount > 0)
				{
					DrawTree(child);
				}
			}
		}
	}
}
#endif // UNITY_EDITOR