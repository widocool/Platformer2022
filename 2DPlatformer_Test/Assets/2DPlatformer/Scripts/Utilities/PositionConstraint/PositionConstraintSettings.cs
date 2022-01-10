namespace GSGD2.Utilities
{
	using UnityEngine;
	using UnityEngine.Animations;

	[System.Serializable]
	public class PositionConstraintSettings
	{
		[SerializeField] private Axis _translationAxis;
		[SerializeField] private Vector3 _translationAtRest;
		[SerializeField] private Vector3 _translationOffset;

		public Axis TranslationAxis => _translationAxis;
		public Vector3 TranslationAtRest => _translationAtRest;
		public Vector3 TranslationOffset => _translationOffset;

		public void CacheSettings(PositionConstraint positionConstraint)
		{
			_translationAxis = positionConstraint.translationAxis;
			_translationAtRest = positionConstraint.translationAtRest;
			_translationOffset = positionConstraint.translationOffset;
		}

		public void ResetSettings(PositionConstraint positionConstraint)
		{
			positionConstraint.translationAxis = _translationAxis;
			positionConstraint.translationAtRest = _translationAtRest;
			positionConstraint.translationOffset = _translationOffset;
		}

	}
}