using System;
using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Character;
using Heph.Scripts.Combat.Card;
using Heph.Scripts.Managers.Dialogue;
using Heph.Scripts.Managers.Game;
using Heph.Scripts.Utils.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

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

        // DEVIATION RELATED VARS
        public DEVIATION_CONTROL currentDeviationControl = DEVIATION_CONTROL.NONE;
        public CARD_TYPE currentExpectationCardType = CARD_TYPE.NONE;
        public Random randomGen;
        [SerializeField] public List<BaseCard> societalFailurePotentialDeckAdditions;
        public DeckHandler societalFailurePotentialAdditionDeckHandler;

        public bool isWaitingOnCardSelection = false;
        
        #region Prefab References

        public GameObject damagePopup;

        #endregion

        public void InitBattle(FighterData playerData, FighterData enemyData, DEVIATION_CONTROL deviationControl)
        {
            // deviation handling
            currentDeviationControl = deviationControl;
            societalFailurePotentialAdditionDeckHandler =
                new DeckHandler(null, societalFailurePotentialDeckAdditions);
            
            // Setup game manager ref
            _gameManager = FindObjectOfType<GameManager>();
            
            // Setup player and enemy fighter handlers
            player = gameObject.AddComponent<FighterHandler>();
            enemy = gameObject.AddComponent<FighterHandler>();
            player.Setup(playerData, true, currentDeviationControl, this);
            enemy.Setup(enemyData, false, currentDeviationControl, this);

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
            if (currentDeviationControl == DEVIATION_CONTROL.SECOND ||
                currentDeviationControl == DEVIATION_CONTROL.BOTH)
            {
                CombatEventsManager.Instance.FinishedDraftingCardAction += HandleFighterFinishedDrafting;
            }
            
            Debug.Log("Player magical defense is (for test): " + player.magicalDefense.Value);
            Debug.Log("Enemy magical defense is (for test): " + enemy.magicalDefense.Value);

            highestFighterDesire = (player.desire.Value >= enemy.desire.Value) ? player.desire.Value : enemy.desire.Value;

            randomGen = new Random(); 
            
            SetState(new BeginCombatState(this));
        }

        private void MoveToResolveState()
        {
            if (GetState().GetType() != typeof(SelectAbilitiesState)) return;
            Debug.Log("Move to resolve state called");
            SetState(new ResolveAbilitiesState(this));
        }
        
        public IEnumerator MoveToNextRound()
        {
            Debug.Log("Battle system is attempting to move to next round");
            Debug.Log(player == null ? "In move to next round, player is null in battlesystem for some reason." : "PlayerRef is not null in battle system");
            Debug.Log(enemy == null? "In move to next round, enemy is null in battlesystem for some reason." : "EnemyRef is not null in battle system");
            Debug.Log("blablabla");

            if (currentDeviationControl == DEVIATION_CONTROL.SECOND ||
                currentDeviationControl == DEVIATION_CONTROL.BOTH)
            {
                Debug.Log("We are checking for second deviation stuff!");
                player.CheckAndSetForSecondDeviationChanges();
                enemy.CheckAndSetForSecondDeviationChanges();
            
                // Second Deviation Usage
                while (isWaitingOnCardSelection)
                {
                    yield return null;
                }
            }

            // Finish Transition To Next Round
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Attempt to move to next round");
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

        public void HandleFighterFinishedDrafting(bool isPlayer)
        {
            if (!isPlayer) return;
            isWaitingOnCardSelection = false;
            CombatEventsManager.Instance.OnToggleChoicesUI(true, true, false);
        }
        
        private void ResetAfterBattle()
        {
            Destroy(player);
            Destroy(enemy);
            dialogueManager.ExitStory();
            currentCombatRound = 0;
            currentRoundAction = 0;
            highestFighterDesire = 0; 
            _gameManager.MoveToResultsScreen();
        }

        private void HandleFighterDefeat(string fighterID)
        {
            _gameManager.lastCombatResult = fighterID != "player" ? "Player won!" : "Player defeated!";
            ResetAfterBattle();
        }
    }
}
