namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Component that translate mouse position into world position. It must be setup with a invisible plane on the xy gameplay axis to receive raycast event (LayerMask can filter the raycast)
	/// </summary>
	[SelectionBase]
	public class MouseToWorld2D : MonoBehaviour
	{
		[SerializeField]
		private Transform _target = null;

		[SerializeField]
		private LayerMask _layerMask = 0;

		private Camera _camera = null;

		[SerializeField]
		public Vector3 TargetPosition => _target.position;

		private void OnEnable()
		{
			_camera = LevelReferences.Instance.Camera;
			_target.gameObject.SetActive(true);
		}

		private void OnDisable()
		{
			_target.gameObject.SetActive(false);
		}

		private void Update()
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _layerMask) == true)
			{
				Vector3 finalPosition = hit.point;
				_target.transform.position = finalPosition;
			}
		}
	}
}