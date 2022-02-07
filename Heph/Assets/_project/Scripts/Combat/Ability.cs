using Heph.Scripts.Character;
using UnityEngine;

namespace Heph.Scripts.Combat
{
    public class Ability : ScriptableObject
    {
        public string title;
        public string description;
        public AbilityType type;
        public int desireCost;

        public virtual void Activate(FighterHandler owner, FighterHandler target) { }
    }
}
