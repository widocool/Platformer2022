#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Editor_AxisDrawer : MonoBehaviour
{
	[SerializeField]
	private int _axisLength = 10;

	[SerializeField]
	private int _axisUnit = 1;

	[SerializeField]
	private Color _upColor = Color.green;

	[SerializeField]
	private Color _rightColor = Color.red;

	private int _infiniteLoopPreventer = 0;

	public int AxisLength => _axisLength;
	public int AxisUnit => _axisUnit;

	private void OnDrawGizmos()
	{
		Vector3 zeroPosition = transform.position;
		Handles.Label(zeroPosition - new Vector3(0.5f, 0.5f, 0f), "0");

		DrawLine(zeroPosition, Vector3.up * _axisLength, _upColor);
		DrawLine(zeroPosition, Vector3.right * _axisLength, _rightColor);

		DrawUnits(zeroPosition, Vector3.up, _upColor);
		DrawUnits(zeroPosition, Vector3.right, _rightColor);
	}

	private void DrawUnits(Vector3 zeroPosition, Vector3 direction, Color color)
	{
		Vector3 vectorUnit = direction * _axisUnit;
		Vector3 marchingVector = vectorUnit;

		Vector3 endPosition = _axisLength * (direction == Vector3.up ? Vector3.right : Vector3.up);

		Vector3 labelStartPositionOffset = (direction == Vector3.up ? Vector3.right : Vector3.up) * -0.5f;

		int i = 1;
		while ((++_infiniteLoopPreventer < 1000) && marchingVector.magnitude <= _axisLength)
		{
			var startPosition = zeroPosition + marchingVector;

			Handles.Label(startPosition + labelStartPositionOffset, i.ToString());
			DrawLine(startPosition, zeroPosition + marchingVector + endPosition, color);
			marchingVector += vectorUnit;
			i++;
		}
		ResetInfiniteLoop();
	}

	private void DrawLine(Vector3 from, Vector3 to, Color color)
	{
		Color previousColor = Gizmos.color;
		Gizmos.color = color;
		Gizmos.DrawLine(from, to);
		Gizmos.color = previousColor;
	}

	private void OnValidate()
	{
		_axisUnit = Mathf.RoundToInt(Mathf.Clamp(_axisUnit, 1f, _axisLength));
		_axisLength = Mathf.RoundToInt(Mathf.Clamp(_axisLength, 2f, float.MaxValue));
	}

	[ContextMenu("Reset infinite loop")]
	private void ResetInfiniteLoop()
	{
		_infiniteLoopPreventer = 0;
	}
}
#endif //UNITY_EDITOR
