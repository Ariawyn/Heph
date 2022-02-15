using System;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat.Ability
{
    [CreateAssetMenu(menuName = "Ability/Attack Ability", fileName = "New Attack Ability", order = 0)]
    class AttackAbility : BaseAbility
    {
        public int amount;
        public bool physical_damage;
        
        public override bool Activate(BaseCard card)
        {
            card.targetRef.HandleDamage(physical_damage? card.ownerRef.physicalAttack.Value : card.ownerRef.magicalAttack.Value, physical_damage);
            return true;
        }
    }
}