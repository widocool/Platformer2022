namespace GSGD2.Utilities
{
	using Cinemachine;
	using GSGD2;
	using UnityEngine;

	/// <summary>
	/// Component that automatically assign <see cref="GSGD2.Player.CameraAim"/> to cinemachine's vCam. Used to reduce dragndrop references when switching player.
	/// </summary>
	public class CinemachineFollowTargetSetter : MonoBehaviour
	{
		[SerializeField]
		private CinemachineVirtualCamera _vCam = null;

		private void Start()
		{
			if (LevelReferences.Instance.PlayerReferences.TryGetCameraAimController(out Player.CameraAimController cameraAimController) == true)
			{
				_vCam.Follow = cameraAimController.CameraAim.transform;
			}
		}
	}

}