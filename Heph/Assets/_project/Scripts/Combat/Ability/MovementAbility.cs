using System;
using Heph.Scripts.Combat.Card;
using UnityEngine;
using UnityEngine.Serialization;

namespace Heph.Scripts.Combat.Ability
{
    [CreateAssetMenu(menuName = "Ability/Movement Ability", fileName = "New Movement Ability", order = 0)]
    class MovementAbility : BaseAbility
    {
        public bool leftMovement;
        public int amountOfSpaces;
        
        public override bool Activate(BaseCard owner)
        {
            if (owner.ownerRef.currentArenaSpaceIndex == 0) return false;
            if (leftMovement)
            {
                if (owner.ownerRef.currentArenaSpaceIndex > 0) amountOfSpaces += 1;
                var newIndex = owner.ownerRef.currentArenaSpaceIndex - amountOfSpaces;
                if (newIndex <= -6) newIndex = -6;
                CombatEventsManager.Instance.OnFighterMovementAction(owner.ownerRef.isPlayerOwned, newIndex);
                return true;
            }
            else
            {
                if (owner.ownerRef.currentArenaSpaceIndex < 0) amountOfSpaces += 1;
                var newIndex = owner.ownerRef.currentArenaSpaceIndex + amountOfSpaces;
                if (newIndex >= 6) newIndex = 6;
                CombatEventsManager.Instance.OnFighterMovementAction(owner.ownerRef.isPlayerOwned, newIndex);
                return true;
            }
        }
    }
}