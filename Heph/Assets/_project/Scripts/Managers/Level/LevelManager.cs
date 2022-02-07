using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Heph.Scripts.Managers.Level
{
	public class LevelManager : MonoBehaviour
	{
		private static LevelManager _instance;

		private void Awake()
		{
			if(_instance == null)
			{
				_instance = this;
			}
			else if(_instance != this)
			{
				Destroy(gameObject);
			}
		}

		public void LoadLevelFromInt(int sceneToLoadIndex)
		{
			var pathToSceneFile = SceneUtility.GetScenePathByBuildIndex(sceneToLoadIndex);
			var slash = pathToSceneFile.LastIndexOf('/');
			var fullSceneName = pathToSceneFile.Substring(slash + 1);
			var dot = fullSceneName.LastIndexOf('.');

			var sceneName = fullSceneName.Substring(0, dot);
			Debug.Log("Loading: " + sceneName + " from integer val");
			LoadLevel(sceneName);
		}

		public void LoadLevel(string levelName, bool skipIntro = false, Action doAfterLoading = null)
		{
			StartCoroutine(LoadLevelAsync(levelName, skipIntro, doAfterLoading));
		}

		public static IEnumerator LoadLevelAsync(string levelName, bool skipIntro, Action doAfterLoading)
		{
			SceneManager.LoadScene(levelName);

			yield return new WaitForSeconds(1f);

			doAfterLoading?.Invoke();
		}
	}
}