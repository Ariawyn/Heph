using Heph.Scripts.Character;
using Heph.Scripts.Utils.StateMachine;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    public class BattleSystem : StateMachine
    {
        private FighterHandler _player;
        private FighterHandler _enemy;

        public int currentCombatRound = 0;
        public int currentRoundAction = 0;

        public int highestFighterDesire = 0;

        public int maxRounds = 15;

        public void InitBattle(FighterHandler playerHandler, FighterHandler enemyHandler)
        {
            _player = playerHandler;
            _enemy = enemyHandler;

            currentCombatRound++;
            CombatEventsManager.Instance.OnRoundTick();

            Debug.Log("Player magical defense is (for test): " + _player.magicalDefense.Value);
            Debug.Log("Enemy magical defense is (for test): " + _enemy.magicalDefense.Value);

            highestFighterDesire = (_player.desire.Value >= _enemy.desire.Value) ? _player.desire.Value : _enemy.desire.Value;

            SetState(new BeginCombatState(this));
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
            _player = null;
            _enemy = null;
            currentCombatRound = 0;
            currentRoundAction = 0;
            highestFighterDesire = 0;
        }
    }
}
