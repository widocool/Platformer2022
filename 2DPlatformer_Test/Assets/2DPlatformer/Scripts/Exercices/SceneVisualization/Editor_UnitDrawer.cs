#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Editor_UnitDrawer : MonoBehaviour
{
	[SerializeField]
	private float _labelOffset = 0.5f;

	[SerializeField]
	private int _fontSize = 13;

	private Editor_AxisDrawer editorAxisDrawer = null;

	private void OnEnable()
	{
		editorAxisDrawer = GetComponentInParent<Editor_AxisDrawer>();
		if (editorAxisDrawer == null)
		{
			enabled = false;
		}
	}

	private void OnDrawGizmos()
	{
		var zeroPosition = transform.position;

		float xPos = transform.position.x;
		float yPos = transform.position.y;

		var style = EditorStyles.whiteLargeLabel;
		style.fontSize = _fontSize;

		string label = $"({xPos};{yPos})";
		Handles.Label(zeroPosition + new Vector3(_labelOffset, _labelOffset, 0f), label, style);
	}
}
#endif