using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Heph.Scripts.Managers.Level;

public class TutorialMenu : MonoBehaviour
{
	private LevelManager _levelManager;

	[SerializeField] List<GameObject> menuScreens;
	[SerializeField] Button advanceButton;
	int index = 0;

	// Start is called before the first frame update
	void Start()
	{
		_levelManager = FindObjectOfType<LevelManager>();
		ShowTutorial();
	}

	public void ButtonPressed()
	{
		index++;
		if(index < menuScreens.Count)
			ShowTutorial();
		else
			PlayGame();
	}

	public void ShowTutorial()
	{
		for(int i = 0; i < menuScreens.Count; i++)
			menuScreens[i].gameObject.SetActive(i == index);
	}

	public void PlayGame()
	{
		_levelManager.LoadLevel("Overworld");
	}

}
