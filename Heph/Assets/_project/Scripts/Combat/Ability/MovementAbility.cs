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
                
                // TODO: Figure out better movement handling
                if (newIndex <= -6) newIndex = -6;
                if (newIndex == 0) newIndex = 1;
                CombatEventsManager.Instance.OnFighterMovementAction(owner.ownerRef.isPlayerOwned, newIndex);
                return true;
            }
            else
            {
                // TODO: Figure out better movement handling
                if (owner.ownerRef.currentArenaSpaceIndex < 0) amountOfSpaces += 1;
                var newIndex = owner.ownerRef.currentArenaSpaceIndex + amountOfSpaces;
                if (newIndex >= 6) newIndex = 6;
                if (newIndex == 0) newIndex = -1;
                CombatEventsManager.Instance.OnFighterMovementAction(owner.ownerRef.isPlayerOwned, newIndex);
                return true;
            }
        }
    }
}