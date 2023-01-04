using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Managers.Game;
using UnityEngine;
using UnityEngine.UI;
using Heph.Scripts.Managers.Level;

public class TutorialMenu : MonoBehaviour
{
	private LevelManager _levelManager;
	private GameManager _gameManager;
	
	[SerializeField] List<GameObject> menuScreens;
	[SerializeField] Button advanceButton;
	int index = 0;
	[SerializeField] private bool loadsIntoBattle;

	public string levelToLoad = "Overworld";

	// Start is called before the first frame update
	void Start()
	{
		_levelManager = FindObjectOfType<LevelManager>();
		_gameManager = FindObjectOfType<GameManager>();
		ShowTutorial();
	}

	public void ButtonPressed()
	{
		index++;
		if(index < menuScreens.Count)
			ShowTutorial();
		else
			PlayLevel();
	}

	public void ShowTutorial()
	{
		for(int i = 0; i < menuScreens.Count; i++)
			menuScreens[i].gameObject.SetActive(i == index);
	}

	public void PlayLevel()
	{
		if (loadsIntoBattle)
		{
			_gameManager.StartBattleFromTutorial();
		}
		else
		{
			_levelManager.LoadLevel(levelToLoad);
		}
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
