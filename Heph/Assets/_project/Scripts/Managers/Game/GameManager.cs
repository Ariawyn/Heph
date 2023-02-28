using System;
using System.Collections.Generic;
using System.Data;
using Heph.Scripts.Character;
using Heph.Scripts.Combat;
using Heph.Scripts.Managers.Level;
using UnityEngine;

namespace Heph.Scripts.Managers.Game
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager _instance;

		private LevelManager _levelManager;

		private const GAME_STATE GameState = GAME_STATE.SPLASH;

		public FighterDataEntry[] characterDataArray;
		private Dictionary<string, FighterData> _characterData;

		[NonSerialized] private string currentEnemyFighter = "";
		[NonSerialized] private bool hasSeenCombatTutorial = false;

		[NonSerialized] public string lastCombatResult = "none";

	public StoryLibrary storylibrary;

		public DEVIATION_CONTROL currentDeviationsActive = DEVIATION_CONTROL.NONE;
		
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

		private void Start()
		{
			_characterData = new Dictionary<string, FighterData>();
			foreach (var entry in characterDataArray)
			{
				_characterData.Add(entry.ID, entry.data);
			}
			_levelManager = FindObjectOfType<LevelManager>();
			storylibrary = GetComponent<StoryLibrary>();
		}

		public void StartBattle(string enemyFighterID)
		{
			currentEnemyFighter = enemyFighterID;
			if (!hasSeenCombatTutorial)
			{
				switch(currentDeviationsActive)
				{
					case DEVIATION_CONTROL.NONE:
						_levelManager.LoadLevel("TutorialCombat");
						break;
					case DEVIATION_CONTROL.FIRST:
						_levelManager.LoadLevel("TutorialDeviation1");
						break;
					case DEVIATION_CONTROL.SECOND:
						_levelManager.LoadLevel("TutorialDeviation2");
						break;
					case DEVIATION_CONTROL.BOTH:
						_levelManager.LoadLevel("TutorialDeviationBoth");
						break;
					default:
						_levelManager.LoadLevel("TutorialCombat");
						break;
				}
			}
			else
			{
				StartCoroutine(LevelManager.LoadLevelAsync("Combat", true, HandleLoadingBattle));	
			}
		}

		public void StartBattleFromTutorial()
		{
			StartCoroutine(LevelManager.LoadLevelAsync("Combat", true, HandleLoadingBattle));	
		}

		private void HandleLoadingBattle()
		{
			var battleControllerObject = GameObject.FindWithTag("BattleController");
			battleControllerObject.TryGetComponent<BattleSystem>(out var battleSystem);
			
			if (battleSystem == null) return;
			Debug.Log("Passing in fighter data");

			var playerData = _characterData["player"];
			var enemyData = _characterData[currentEnemyFighter];
			battleSystem.InitBattle(playerData, enemyData, currentDeviationsActive);
			CombatEventsManager.Instance.OnBattleStarted();
		}

		public void MoveToOverworld()
		{
			StartCoroutine(LevelManager.LoadLevelAsync("Overworld", true, null));
		}

		public void MoveToResultsScreen()
		{
			StartCoroutine(LevelManager.LoadLevelAsync("ResultsScreen", true, null));
		}

		public void UpdateDeviation(DEVIATION_CONTROL control_val)
		{
			currentDeviationsActive = control_val;
		}
		

		// Update is called once per frame
		private void Update()
		{
			switch(GameState)
			{
				case GAME_STATE.SPLASH:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Quit()
		{
			// Handle other stuff?
			#if UNITY_EDITOR
						UnityEditor.EditorApplication.isPlaying = false;
			#else
			            Application.Quit();
			#endif
		}
	}
}
