using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Combat.Card;
using UnityEngine;

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
