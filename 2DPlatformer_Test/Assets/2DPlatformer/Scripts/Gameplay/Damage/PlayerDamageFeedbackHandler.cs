namespace GSGD2.Gameplay
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using GSGD2.Utilities;
	using UnityEngine.InputSystem;

	/// <summary>
	/// Component that listen a Damageable and react to its TakeDamage event. It can swap materials, pulse the emissive, instantiate particles and rumble the controller.
	/// It was targeted to be generic but it contains gamepad related stuffs that should be removed if used generically.
	/// </summary>
	public class PlayerDamageFeedbackHandler : MonoBehaviour
	{
		[Header("Rumble")]
		[SerializeField]
		private bool _canRumble = true;

		[SerializeField]
		private float _lowFreq = 0.25f;

		[SerializeField]
		private float _highFreq = 0.75f;

		[Header("Material swap")]
		[SerializeField]
		private MaterialDescription _materialSwap = null;

		[SerializeField]
		private float _swapMaterialDuration = 1f;

		[SerializeField]
		private MeshRenderer[] _meshRenderersToSwapMaterial = null;

		[Header("Pulse emissive")]
		[SerializeField]
		private PulseEmissive _pulseEmissive = null;

		[SerializeField]
		private MeshRenderer[] _meshRenderersToPulseEmissive = null;

		[Header("Particles")]
		[SerializeField]
		private ParticleInstancier _takeDamageParticle = null;

		private MaterialDescription _materialSwapInstance = null;
		private Coroutine _currentRoutine = null;

		// Do no erase : called by a UnityEvent
		public void OnTakeDamage()
		{
			if (enabled == false) return;
			if (_currentRoutine != null)
			{
				StopCoroutine(_currentRoutine);
				_currentRoutine = null;
			}
			_currentRoutine = StartCoroutine(WaitForFeedback());
		}

		// Do no erase : called by a UnityEvent
		public void OnTakeDamageAndHealthBelowZero()
		{
		}

		private void Awake()
		{

		}

		private void OnEnable()
		{
			_materialSwapInstance = _materialSwap.CreateNew();
			_materialSwapInstance.SetCachedOriginalMaterial(_meshRenderersToSwapMaterial);
		}

		private IEnumerator WaitForFeedback()
		{
			bool canChangeMaterial = _meshRenderersToSwapMaterial.Length > 0;
			bool canPulseEmissive = _meshRenderersToPulseEmissive.Length > 0;

			if (Gamepad.current != null && _canRumble == true)
			{
				Gamepad.current.SetMotorSpeeds(_lowFreq, _highFreq);
			}

			if (canChangeMaterial == true)
			{
				_materialSwapInstance.ChangeMaterial(_meshRenderersToSwapMaterial, true);
			}
			if (canPulseEmissive == true)
			{
				_pulseEmissive.ResetPulse();
			}
			if (_takeDamageParticle != null)
			{
				_takeDamageParticle.Instantiate();
			}

			var currentTime = 0f;
			while (currentTime < _swapMaterialDuration)
			{
				if (canPulseEmissive == true)
				{
					_pulseEmissive.Pulse(_meshRenderersToPulseEmissive);
				}
				currentTime += Time.deltaTime;
				yield return null;
			}
			ResetFeedbacks(canChangeMaterial, canPulseEmissive);
		}

		private void ResetFeedbacks(bool resetChangeMaterial, bool resetPulseEmissive)
		{
			if (resetChangeMaterial == true)
			{
				_materialSwapInstance.ChangeMaterial(_meshRenderersToSwapMaterial, false);
			}
			if (resetPulseEmissive == true)
			{
				_pulseEmissive.ResetPulse();
			}
			if (Gamepad.current != null && _canRumble == true)
			{
				Gamepad.current.SetMotorSpeeds(0f, 0f);
			}
		}
	}
}
