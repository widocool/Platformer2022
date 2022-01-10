namespace GSGD2.Utilities
{
	using UnityEngine;
	using UnityEngine.Animations;

	/// <summary>
	/// Conveniance class with a nice inspector to quickly setup raycast.
	/// <see cref="Raycast(out RaycastHit, QueryTriggerInteraction, bool)"/> or <see cref="RaycastAll(out UnityEngine.RaycastHit[], UnityEngine.QueryTriggerInteraction, bool)"/> must be called in order to get a result.
	/// The gizmos will be green if hit, red if not it, and can be adjusted with <see cref="SetResultFalse()"/>.
	/// </summary>
	[System.Serializable]
	public class Raycaster
	{
		[SerializeField]
		private Transform fromTransform;

		[SerializeField]
		private float _maxDistance = 1f;

		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField]
		private bool _drawGizmos = false;

		private bool _lastResult = false;

		public float MaxDistance => _maxDistance;
		public bool LastResult => _lastResult;

		public Vector3 WorldPosition => fromTransform.position;

		private float _cachedMaxDistance = -1f;

		public void Initialize()
		{
			if (fromTransform.GetComponent<PositionConstraint>() != null)
			{
				fromTransform.parent = null;
			}
		}

		public void SetMaxDistance(float newDistance)
		{
			if (_cachedMaxDistance == -1)
			{
				_cachedMaxDistance = _maxDistance;
			}
			_maxDistance = newDistance;
		}

		public void ResetMaxDistance()
		{
			if (_cachedMaxDistance != -1)
			{
				_maxDistance = _cachedMaxDistance;
			}
		}

		public bool Raycast(out RaycastHit hit, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore, bool debug = false)
		{
			bool result = Physics.Raycast(fromTransform.position, fromTransform.forward, out hit, _maxDistance, _layerMask, queryTriggerInteraction);
			if (debug)
			{
				Debug.LogFormat("Debug Raycast {0}", hit.transform != null ? hit.transform.name : "found nothing.");
			}
			return result;
		}

		public bool RaycastAll(out RaycastHit[] hits, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore, bool debug = false)
		{
			hits = Physics.RaycastAll(fromTransform.position, fromTransform.forward, _maxDistance, _layerMask, queryTriggerInteraction);
			if (debug)
			{
				DebugRaycast(ref hits);
			}
			return _lastResult = hits.Length > 0;
		}

		public string GetDebugString(RaycastHit[] hits)
		{
			string message = string.Empty;
			for (int i = 0; i < hits.Length; i++)
			{
				var hit = hits[i];
				message = $"{message} | {hit.transform.name}";
			}
			return message;
		}

		public void DebugRaycast(ref RaycastHit[] hits)
		{
			string msg = "Debug Raycast : " + GetDebugString(hits);
			if (msg != string.Empty)
			{
				Debug.Log(msg);
			}
		}

		//filtered result may be not what Raycaster know
		public void SetResultFalse()
		{
			_lastResult = false;
		}

		public void DrawGizmos()
		{
			DrawGizmos(Color.green, Color.red);
		}

		public void DrawGizmos(Color goodResultColor, Color badResultColor)
		{
			if (_drawGizmos == false || fromTransform == null) return;
			var startPosition = fromTransform.position;
			var endPosition = startPosition + fromTransform.forward * _maxDistance;
			Debug.DrawLine(startPosition, endPosition, _lastResult == true ? goodResultColor : badResultColor);
		}
	}
}