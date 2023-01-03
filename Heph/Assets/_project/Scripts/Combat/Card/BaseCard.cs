using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Heph.Scripts.Character;
using UnityEngine;

using Heph.Scripts.Combat.Ability;
using UnityEngine.WSA;

namespace Heph.Scripts.Combat.Card
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/Basic")]
    public class BaseCard : ScriptableObject
    {
        public new string name;
        public string description;
        public Sprite artwork;
        public CARD_TYPE type;

        // Technically both abilities and cards have a desireCost but the cards desireCost is prioritized
        public int desireCost;
        public BaseAbility[] abilities;

        public FighterHandler ownerRef;
        public FighterHandler targetRef;
        
        public IEnumerator Activate(FighterHandler owner, FighterHandler target)
        {
            ownerRef = owner;
            targetRef = target;
            
            // Technically we should have a viability check before we activate everything, but i am unsure how to do that atm... TODO: Figure it out
            // TODO: Also, we need to have the coroutine wait for the execution of all abilities
            abilities.All(ability => ability.Activate(this));

            yield break;
        }
        
    }
}
