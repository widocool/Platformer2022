namespace GSGD2.Utilities
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	/// <summary>
	/// Basis component class that can load a scene based on its name. 
	/// </summary>
	public class LoadSceneComponent : MonoBehaviour
	{
		[SerializeField]
		private bool _loadAtOnEnable = false;

		[SerializeField]
		private string _sceneName = string.Empty;

		[SerializeField]
		private LoadSceneMode _mode = LoadSceneMode.Single;

		private void OnEnable()
		{
			if (_loadAtOnEnable == true)
			{
				LoadScene();
			}
		}

		public void LoadScene()
		{
			LoadScene(_sceneName, _mode);
		}

		public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
		{
			SceneManager.LoadSceneAsync(name, mode);
		}
	}

}