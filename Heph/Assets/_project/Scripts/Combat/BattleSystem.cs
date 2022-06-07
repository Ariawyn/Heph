using System;
using Heph.Scripts.Character;
using Heph.Scripts.Combat.Card;
using Heph.Scripts.Managers.Dialogue;
using Heph.Scripts.Managers.Game;
using Heph.Scripts.Utils.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Heph.Scripts.Combat
{
    public class BattleSystem : StateMachine
    {
        [NonSerialized] private GameManager _gameManager;
        
        [NonSerialized] public FighterHandler player;
        [NonSerialized] public FighterHandler enemy;

        public ArenaHandler Arena;

        [NonSerialized] public int currentCombatRound = 0;
        [NonSerialized] public int currentRoundAction = 0;

        [NonSerialized] public int highestFighterDesire = 0;

        [NonSerialized] private readonly int maxRounds = 15;

        public DialogueManager dialogueManager;
        public bool isWaitingOnDialogue = false;

        public CombatUIHandler combatUI;

        public void InitBattle(FighterData playerData, FighterData enemyData)
        {
            // Setup game manager ref
            _gameManager = FindObjectOfType<GameManager>();
            
            // Setup player and enemy fighter handlers
            player = gameObject.AddComponent<FighterHandler>();
            enemy = gameObject.AddComponent<FighterHandler>();
            player.Setup(playerData, true);
            enemy.Setup(enemyData, false);
            
            // Setup INK story for battle
            var story = _gameManager.storylibrary.GetCurrentStoryForEnemy(enemyData.ID);
            dialogueManager.StartStory(story);

            Debug.Log("Init battle");
            if (player == null) { Debug.Log("In init battle func in battle system, player is null in battlesystem for some reason."); }
            if (enemy == null) { Debug.Log("In init battle func in battle system, enemy is null in battlesystem for some reason."); }
            
            currentCombatRound++;
            CombatEventsManager.Instance.OnRoundTick();

            CombatEventsManager.Instance.SelectionConfirmButtonEvent += MoveToResolveState;
            CombatEventsManager.Instance.FighterDefeatedAction += HandleFighterDefeat;
            
            Debug.Log("Player magical defense is (for test): " + player.magicalDefense.Value);
            Debug.Log("Enemy magical defense is (for test): " + enemy.magicalDefense.Value);

            highestFighterDesire = (player.desire.Value >= enemy.desire.Value) ? player.desire.Value : enemy.desire.Value;

            SetState(new BeginCombatState(this));
        }

        private void MoveToResolveState()
        {
            if (GetState().GetType() != typeof(SelectAbilitiesState)) return;
            Debug.Log("Move to resolve state called");
            SetState(new ResolveAbilitiesState(this));
        }
        
        public void MoveToNextRound()
        {
            Debug.Log("Battle system is attempting to move to next round");
            Debug.Log(player == null ? "In move to next round, player is null in battlesystem for some reason." : "PlayerRef is not null in battle system");
            Debug.Log(enemy == null? "In move to next round, enemy is null in battlesystem for some reason." : "EnemyRef is not null in battle system");
            Debug.Log("blablabla");

            if(currentCombatRound < maxRounds)
            { 
                currentCombatRound++;
                CombatEventsManager.Instance.OnRoundTick();
                
                currentRoundAction = 0;
                SetState(new SelectAbilitiesState(this)); 
            }
            else
            {
                //TODO: Finish combat due to time out.
                Debug.Log("Combat time out");
                ResetAfterBattle();
            }
        }

        private void ResetAfterBattle()
        {
            Destroy(player);
            Destroy(enemy);
            dialogueManager.ExitStory();
            currentCombatRound = 0;
            currentRoundAction = 0;
            highestFighterDesire = 0; 
            _gameManager.MoveToOverworld();
        }

        private void HandleFighterDefeat(string fighterID)
        {
            Debug.Log(fighterID != "player" ? "Player won!" : "Player defeated!");
            ResetAfterBattle();
        }
    }
}
