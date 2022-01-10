#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Editor_VectorDrawer : MonoBehaviour
{
	[SerializeField]
	private Transform _endVector3f = null;

	[SerializeField]
	private Color _color = Color.blue;

	[SerializeField]
	private bool _drawMagnitude = false;

	[SerializeField]
	private bool _drawRadius = false;

	private float _radius = 1f;

	private void OnDrawGizmos()
	{
		if (_endVector3f == null) return;

		var zeroPosition = transform.position;
		var endPosition = _endVector3f.position;
		DrawLine(zeroPosition, endPosition, _color);
		DrawArrow(zeroPosition, endPosition - zeroPosition, 1, _color);

		if (_drawMagnitude == true)
		{
			var vector = zeroPosition - _endVector3f.position;
			var magnitude = vector.magnitude;
			Handles.Label(Vector3.Lerp(zeroPosition, endPosition, 0.5f), magnitude.ToString("0.0"));
		}
		if (_drawRadius == true)
		{
			Handles.DrawWireDisc(zeroPosition, Vector3.forward, _radius);
		}
	}

	private void DrawLine(Vector3 from, Vector3 to, Color color)
	{
		Color previousColor = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawLine(from, to);
		Gizmos.color = previousColor;
	}

	private void DrawArrow(Vector3 startPosition, Vector3 direction, float size, Color color)
	{
		Color previousColor = Handles.color;
		Handles.color = color;
		Handles.ArrowHandleCap(0, startPosition, Quaternion.LookRotation(direction), size, EventType.Repaint);
		Handles.color = previousColor;
	}
}
#endif