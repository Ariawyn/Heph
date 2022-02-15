using System;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat.Ability
{
    [CreateAssetMenu(menuName = "Ability/Shield Ability", fileName = "New Shield Ability", order = 0)]
    class ShieldAbility : BaseAbility
    {
        public int amount;
        
        public override bool Activate(BaseCard owner)
        {
            throw new NotImplementedException();
        }
    }
}