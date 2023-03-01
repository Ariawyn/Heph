using System;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Combat.Ability
{
    [Serializable]
    public abstract class BaseAbility : ScriptableObject
    {
        public string title;
        public string description;

        public abstract bool Activate(BaseCard owner);
    }
}