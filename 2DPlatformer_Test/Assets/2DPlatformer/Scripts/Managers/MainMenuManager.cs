namespace GSGD2.Utilities
{
	using System.Collections;
	using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
	using UnityEditor.UI;
	using UnityEngine.SceneManagement;
	using UnityEngine.EventSystems;
#endif // UNITY_EDITOR

	/// <summary>
	/// Quick class used to handle MainMenu scene.
	/// </summary>
	public class MainMenuManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject _mainMenuCanvas = null;
		
		[SerializeField]
		private GameObject _levelSelectCanvas = null;

		[SerializeField]
		private GameObject _playButton = null;

		[SerializeField]
		private GameObject _level01Button = null;

		public void Quit()
		{
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif //UNITY_EDITOR
		}



		public void SelectLevel()
		{
			_mainMenuCanvas.gameObject.SetActive(false);
			_levelSelectCanvas.gameObject.SetActive(true);
			EventSystem.current.SetSelectedGameObject(_level01Button.gameObject);
		}

		public void MainMenu()
        {
			_mainMenuCanvas.gameObject.SetActive(true);
			_levelSelectCanvas.gameObject.SetActive(false);

			EventSystem.current.SetSelectedGameObject(_playButton.gameObject);
		}
	}
}