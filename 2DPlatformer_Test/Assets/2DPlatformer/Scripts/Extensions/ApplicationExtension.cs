namespace GSGD2.Extensions
{
	using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif //UNITY_EDITOR

	public static class ApplicationExtension
	{
		public static bool IsPlaying
		{
			get
			{
#if UNITY_EDITOR
				return EditorApplication.isPlayingOrWillChangePlaymode;
#else // in build
				return Application.isPlaying;
#endif //UNITY_EDITOR
			}
		}
	}
}