using System;
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
            throw new NotImplementedException();
        }
    }
}