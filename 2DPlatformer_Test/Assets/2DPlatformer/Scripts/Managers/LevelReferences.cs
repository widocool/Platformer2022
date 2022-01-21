namespace GSGD2
{
	using Cinemachine;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using Gameplay;
	using GSGD2.Utilities;
	using GSGD2.Player;
	using GSGD2.UI;
	using UnityEngine.InputSystem;

	/// <summary>
	/// Singleton class (needs to be added only once, but accessing from everywhere). It's purpose is to be only a proxy for other managers or unique component (player, camera related gameobjects, <see cref="PlayerStart"/>, etc) (similar to a "Service Locator" pattern).
	/// </summary>
	public class LevelReferences : Singleton<LevelReferences>
	{
		[SerializeField]
		private PlayerReferences _playerReferences = null;

		[SerializeField]
		private CubeController _player = null;

		[SerializeField]
		private Camera _mainCamera = null;

		[SerializeField]
		private CameraEventManager _cameraEventManager = null;

		[SerializeField]
		private UIManager _uiManager = null;

		[SerializeField]
		private PlayerStart _playerStart = null;

		[SerializeField]
		private MouseToWorld2D _mouseToWorld2D = null;
		
		[SerializeField]
		private LootManager _lootManager = null;

		public PlayerReferences PlayerReferences => _playerReferences;
		public CubeController Player => _player;
		public PlayerStart PlayerStart => _playerStart;
		public Camera Camera => _mainCamera;
		public CameraEventManager CameraEventManager => _cameraEventManager;
		public MouseToWorld2D MouseToWorld2D => _mouseToWorld2D;
		public UIManager UIManager => _uiManager;
		public LootManager LootManager => _lootManager;

		protected override void OnDestroy()
		{
			base.OnDestroy();

			// TODO AL : lazy, redo this properly
			if (Gamepad.current != null)
			{
				Gamepad.current.SetMotorSpeeds(0f, 0f);
			}
		}
	}
}