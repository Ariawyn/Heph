using System;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat.Ability
{
    [CreateAssetMenu(menuName = "Ability/Movement Ability", fileName = "New Movement Ability", order = 0)]
    class MovementAbility : BaseAbility
    {
        public bool left_movement;
        public int amount_of_spaces;
        
        public override bool Activate(BaseCard owner)
        {
            throw new NotImplementedException();
        }
    }
}