using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Heph.Scripts.Combat.Card;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Heph.Scripts.Combat
{
    public class SelectAbilitiesState : BattleState
    {
        public SelectAbilitiesState(BattleSystem battleSystem) : base(battleSystem) { }

        public override IEnumerator Start()
        {
            Debug.Log("Select Abilities State started");

            if (BattleSystemRef.player == null) { Debug.Log("In start of select abilities state, player is null in battlesystem for some reason."); }
            if (BattleSystemRef.enemy == null) { Debug.Log("In start of select abilities state, enemy is null in battlesystem for some reason."); }
            
            // DRAW CARDS
            BattleSystemRef.player.HandleStartTurn();
            BattleSystemRef.enemy.HandleStartTurn();

            CombatEventsManager.Instance.OnSelectAbilitiesStateEntered();
            
            // HANDLE SECOND DEVIATION STUFF IF NEEDED
            if (BattleSystemRef.currentDeviationControl is DEVIATION_CONTROL.SECOND or DEVIATION_CONTROL.BOTH)
            {
                var values = Enum.GetValues(typeof(CARD_TYPE)).Cast<CARD_TYPE>().ToList();
                values.Remove(CARD_TYPE.NONE);
                //if (BattleSystemRef.currentDeviationControl != DEVIATION_CONTROL.BOTH) values.Remove(CARD_TYPE.FLIRT);
                values.Remove(CARD_TYPE.FLIRT);
                var currentRoundExpectation = values[BattleSystemRef.randomGen.Next(values.Count)];
                BattleSystemRef.currentExpectationCardType = currentRoundExpectation;
                Debug.Log("Updated societal expectation of card type: " + BattleSystemRef.currentExpectationCardType);
            }
            
            yield return new WaitForSeconds(1);
        }

        public override void End()
        {
            BattleSystemRef.player.HandleEndTurn();
            BattleSystemRef.enemy.HandleEndTurn();
            BattleSystemRef.combatUI.ClearBoard();
            Debug.Log("Select Abilities State ended");
        }
    }
}
