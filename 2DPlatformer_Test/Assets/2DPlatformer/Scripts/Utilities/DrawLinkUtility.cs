namespace GSGD2.Utilities
{
	using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif //UNITY_EDITOR

	/// <summary>
	/// Editor utility class that can draw Bezier link with a conveniant inspector interface.
	/// </summary>
	[System.Serializable]
	public class DrawLinkUtility
	{
		[SerializeField]
		private bool _drawGizmos = false;

		[SerializeField]
		private Vector3 _bezierStartOffset = Vector3.up;

		[SerializeField]
		private Vector3 _bezierEndOffset = Vector3.up;

		[SerializeField]
		private float _bezierUpTangentOffset = 0.5f;

		[SerializeField]
		private Color _color = Color.blue;

		[SerializeField]
		private float _width = 2f;

		public void DrawBezierLink(Vector3 from, Vector3 to)
		{
#if UNITY_EDITOR
			{
				if (_drawGizmos == false) return;
				Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
				Vector3 startPosition = from + _bezierStartOffset;
				Vector3 endPosition = to + _bezierEndOffset;
				Vector3 offset = Vector3.up * (startPosition.y - endPosition.y + _bezierUpTangentOffset);
				Handles.DrawBezier(startPosition, endPosition, startPosition + offset, endPosition + offset, _color, EditorGUIUtility.whiteTexture, _width);
			}
#endif //UNITY_EDITOR
		}

		public void DrawLink(Vector3 from, Vector3 to)
		{
#if UNITY_EDITOR
			if (_drawGizmos == false) return;
			Color previousColor = Handles.color;
			Handles.color = _color;
			Handles.DrawLine(from + _bezierStartOffset, to + _bezierEndOffset, _width);
			previousColor = Handles.color;

#endif //UNITY_EDITOR
		}
	}
}