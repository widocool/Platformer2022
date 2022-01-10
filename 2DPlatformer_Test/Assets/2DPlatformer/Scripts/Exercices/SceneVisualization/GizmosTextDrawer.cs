#if UNITY_EDITOR
namespace GSGD2.Editor
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	public class GizmosTextDrawer : MonoBehaviour
	{
		[SerializeField]
		private string _label = null;

		[SerializeField]
		private bool _drawGameObjectName = false;

		[SerializeField]
		private Vector3 _offset = Vector3.up;

		[SerializeField]
		private int _fontSize = 13;

		private void OnDrawGizmos()
		{
			var style = new GUIStyle(EditorStyles.label);
			style.fontSize = _fontSize;

			Handles.Label(transform.position + _offset, _drawGameObjectName == true ? transform.name : _label, style);
		}
	} 
}
#endif // UNITY_EDITOR