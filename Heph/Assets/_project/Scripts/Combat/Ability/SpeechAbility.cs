using System;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat.Ability
{
    [CreateAssetMenu(menuName = "Ability/Speech Ability", fileName = "New Speech Ability", order = 0)]
    class SpeechAbility : BaseAbility
    {
        public override bool Activate(BaseCard owner)
        {
            throw new NotImplementedException();
        }
    }
}