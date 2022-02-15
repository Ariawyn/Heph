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

            // DRAW CARDS
            var cardsInHand = BattleSystem.player.DrawCards();
            BattleSystem.combatUI.DisplayHand(cardsInHand);

            yield return new WaitForSeconds(1);
        }

        public override void End()
        {
            BattleSystem.combatUI.ClearHand();
            Debug.Log("Select Abilities State ended");
        }
    }
}
