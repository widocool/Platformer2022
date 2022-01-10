namespace GSGD2.Gameplay
{
	using GSGD2.Player;
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// Player version of <see cref="MovingPlatformStickToGroundReceiver"/>. It handle changing set too falling when a platform push the player away during a wall grab.
	/// </summary>
	public class PlayerMovingPlatformStickToGroundReceiver : MovingPlatformStickToGroundReceiver
	{
		private CubeController _cubeController = null;

		protected override void OnSetOnPlatform(bool isOnPlatform)
		{
			base.OnSetOnPlatform(isOnPlatform);

			if (_cubeController.CurrentState == CubeController.State.WallGrab && isOnPlatform == false)
			{
				_cubeController.ChangeState(CubeController.State.Falling);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			GetComponent<PlayerReferences>().TryGetCubeController(out _cubeController);
		}
	}
}