namespace GSGD2.Utilities
{
	using UnityEngine;

	/// <summary>
	/// Conveniance class that can linearly interpolate between two colors. <see cref="Pulse(UnityEngine.MeshRenderer)"/> methods MUST be called by an Update method.
	/// </summary>
	[System.Serializable]
	public class PulseEmissive
	{
		private const string EMISSION_PARAMETER_NAME = "_EmissionColor";

		[ColorUsage(true, true)]
		[SerializeField]
		private Color startEmissionColor;

		[ColorUsage(true, true)]
		[SerializeField]
		private Color endEmissionColor;

		[SerializeField]
		private float _rate = 2f;

		private float _currentTime = 1f;

		// TODO AL : add a curve to have more control on pulse

		public void ResetPulse()
		{
			_currentTime = 0f;
		}

		public void Pulse(MeshRenderer meshRenderer)
		{
			var value = Mathf.PingPong(_currentTime, 1f);
			Color color = Color.Lerp(startEmissionColor, endEmissionColor, value);
			meshRenderer.material.SetColor(EMISSION_PARAMETER_NAME, color);
			_currentTime += Time.deltaTime * _rate;
		}

		public void Pulse(MeshRenderer[] meshRenderers)
		{
			for (int i = 0; i < meshRenderers.Length; i++)
			{
				var mr = meshRenderers[i];
				Pulse(mr);
			}
		}
	}
}