using System;
using Heph.Scripts.Combat.Card;
using UnityEngine;
using UnityEngine.Serialization;

namespace Heph.Scripts.Combat.Ability
{
    [CreateAssetMenu(menuName = "Ability/Movement Ability", fileName = "New Movement Ability", order = 0)]
    class MovementAbility : BaseAbility
    {
        public bool isRetreat;
        public int amountOfSpaces;
        
        public override bool Activate(BaseCard owner)
        {
            CombatEventsManager.Instance.OnFighterMovementAction(owner.ownerRef.isPlayerOwned, amountOfSpaces, isRetreat);
            return true;
        }
    }
}