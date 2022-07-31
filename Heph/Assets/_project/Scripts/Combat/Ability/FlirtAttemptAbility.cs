using System;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat.Ability
{
    [CreateAssetMenu(menuName = "Ability/Flirt Ability", fileName = "New Flirt Ability", order = 0)]
    class FlirtAttemptAbility : BaseAbility
    {
        public int flirtActionType = 0;
        
        public override bool Activate(BaseCard owner)
        {
            owner.ownerRef.AttemptToRespondToDialogueChoice(flirtActionType, true);
            return true;
        }
    }
}