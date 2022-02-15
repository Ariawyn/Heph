using System;
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

		[HideInInspector] private const GAME_STATE GameState = GAME_STATE.SPLASH;

		// BAD CODE REGION (WE WILL CHANGE THIS WHEN WE GET A SAVE GAME WHERE WE CAN JUST HAVE DATA WITH ID FOR EACH POSSIBLE ENEMY AND START A BATTLE THAT WAY)
		private FighterHandler _tempPlayerData;
		private FighterHandler _tempEnemyData;

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
			_levelManager = FindObjectOfType<LevelManager>();
		}

		public void StartBattle(FighterHandler player, FighterHandler enemy)
		{
			// TEMP UNTIL ID AND SAVE GAME SYSTEM SORTED
			_tempPlayerData = player;
			_tempEnemyData = enemy;

			StartCoroutine(LevelManager.LoadLevelAsync("CombatTestScene", true, HandleLoadingBattle));
		}

		private void HandleLoadingBattle()
		{
			var battleControllerObject = GameObject.FindWithTag("BattleController");
			battleControllerObject.TryGetComponent<BattleSystem>(out var battleSystem);
			
			if (battleSystem == null) return;
			Debug.Log("Passing in fighterhandler data");
			battleSystem.InitBattle(_tempPlayerData, _tempEnemyData);
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
