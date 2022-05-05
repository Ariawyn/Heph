using System;
using Heph.Scripts.Character;
using Heph.Scripts.Combat.Card;
using UnityEngine;
using UnityEngine.WSA;

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