namespace GSGD2.Utilities
{
	using Cinemachine;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Component that can add the player reference to a <see cref="CinemachineTargetGroup"/> instead of drag n dropping it.
	/// </summary>
	public class CinemachineCameraRoomAddPlayerToTargetGroup : MonoBehaviour
	{
		[SerializeField]
		private CinemachineTargetGroup _cinemachineTargetGroup = null;

		private void Start()
		{
			if (LevelReferences.Instance.PlayerReferences.TryGetCameraAimController(out Player.CameraAimController cameraAimController) == true)
			{
				_cinemachineTargetGroup.AddMember(cameraAimController.transform, 1f, 2f);
			}
		}
	}
}