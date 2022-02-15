using System;
using Heph.Scripts.Character;
using Heph.Scripts.Combat.Card;
using Heph.Scripts.Utils.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Heph.Scripts.Combat
{
    public class BattleSystem : StateMachine
    {
        public FighterHandler player;
        public FighterHandler enemy;

        public int currentCombatRound = 0;
        public int currentRoundAction = 0;

        public int highestFighterDesire = 0;

        public int maxRounds = 15;

        public CombatUIHandler combatUI;
        
        // TEMP STUFF;
        public BaseCard testCard;

        public void InitBattle(FighterHandler playerHandler, FighterHandler enemyHandler)
        {
            player = playerHandler;
            enemy = enemyHandler;
            player.SetupForCombat();
            enemy.SetupForCombat();
            player.deck.Add(testCard);
            enemy.deck.Add(testCard);
            
            currentCombatRound++;
            CombatEventsManager.Instance.OnRoundTick();

            CombatEventsManager.Instance.SelectionConfirmButtonEvent += MoveToResolveState;
            
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
            player = null;
            enemy = null;
            currentCombatRound = 0;
            currentRoundAction = 0;
            highestFighterDesire = 0;
        }
    }
}
