using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Managers.Game;
using UnityEngine;
using UnityEngine.UI;
using Heph.Scripts.Managers.Level;

public class ResultMenuHandler : MonoBehaviour
{
	private LevelManager _levelManager;
	private GameManager _gameManager;
	
	[SerializeField] public Button playButton;
	public string levelToLoad = "Overworld";

	// Start is called before the first frame update
	void Start()
	{
		_levelManager = FindObjectOfType<LevelManager>();
		_gameManager = FindObjectOfType<GameManager>();
	}

	public void ButtonPressed()
	{
		PlayLevel();
	}

	public void PlayLevel()
	{
		_levelManager.LoadLevel(levelToLoad);
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
	}
	
}
