using System;
using System.Transactions;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat.Ability
{
    
    [CreateAssetMenu(menuName = "Ability/Recover Ability", fileName = "New Recover Ability", order = 0)]
    class RecoverAbility : BaseAbility
    {
        public int amount;
        
        public override bool Activate(BaseCard owner)
        {
            if (amount + owner.ownerRef.currentHealth.Value > owner.ownerRef.maximumHealth.Value)
            {
                var overhealth = (amount + owner.ownerRef.currentHealth.Value) - owner.ownerRef.maximumHealth.Value;
                owner.ownerRef.currentHealth.Value += owner.ownerRef.maximumHealth.Value;
                owner.ownerRef.currentOverhealth.Value = overhealth;
            }
            else
            {
                owner.ownerRef.currentHealth.Value += amount;
            }
            return true;
        }
    }
}