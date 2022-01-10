namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Scriptable Object that hold a material that used to be swapped on MeshRenderer.
	/// In needs to be instantiated before by <see cref="CreateNew()"/> since it cache the previous materials.
	/// <see cref="GSGD2.Gameplay.CheckpointFeedbackHandler"/> for examples.
	/// </summary>
	[CreateAssetMenu(menuName = "GameSup/MaterialDescription", fileName = "MaterialDescription")]
	public class MaterialDescription : ScriptableObject
	{
		[SerializeField]
		private Material _alternativeMaterial = null;

		private List<Material> _cachedOriginalMaterial = null;

		private bool _isSwapped = false;

		public bool IsSwapped => _isSwapped;

		public MaterialDescription CreateNew() => ScriptableObject.Instantiate<MaterialDescription>(this);

		public void SetCachedOriginalMaterial(MeshRenderer[] meshRenderers)
		{
			if (meshRenderers.Length == 0) return;

			_cachedOriginalMaterial = new List<Material>();
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				var mr = meshRenderers[i];
				_cachedOriginalMaterial.Add(mr.material);
			}
		}

		public bool SwapMaterial(MeshRenderer[] meshRenderers)
		{
			if (CanChangeMaterial(meshRenderers) == false)
			{
				return _isSwapped;
			}

			for (int i = 0; i < meshRenderers.Length; i++)
			{
				var mr = meshRenderers[i];
				mr.material = _isSwapped == true ? _cachedOriginalMaterial[i] : _alternativeMaterial;
			}

			_isSwapped = !_isSwapped;
			return _isSwapped;
		}

		public bool ChangeMaterial(MeshRenderer[] meshRenderers, bool alternativeMaterial)
		{
			if (CanChangeMaterial(meshRenderers) == false)
			{
				return _isSwapped;
			}

			bool result = _isSwapped == false && alternativeMaterial == true
						|| _isSwapped == true && alternativeMaterial == false;
			if (result == true)
			{
				SwapMaterial(meshRenderers);
			}
			return _isSwapped;
		}

		private bool CanChangeMaterial(MeshRenderer[] meshRenderers)
		{
			if (meshRenderers.Length == 0)
			{
				return false;
			}
			if (_cachedOriginalMaterial == null || meshRenderers.Length != _cachedOriginalMaterial.Count)
			{
				Debug.LogError("Can't ChangeMaterial(), you must call SetCachedOriginalMaterial before");
				return false;
			}
			return true;
		}
	}
}