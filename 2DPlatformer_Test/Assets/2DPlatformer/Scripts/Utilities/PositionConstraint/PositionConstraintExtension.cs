namespace GSGD2.Utilities
{
	using UnityEngine.Animations;

	public static class PositionConstraintExtension
	{
		public static void ApplySettings(this PositionConstraint positionConstraint, PositionConstraintSettings positionConstraintSettings)
		{
			positionConstraint.translationAxis = positionConstraintSettings.TranslationAxis;
			positionConstraint.translationAtRest = positionConstraintSettings.TranslationAtRest;
			positionConstraint.translationOffset = positionConstraintSettings.TranslationOffset;
		}
	}
}