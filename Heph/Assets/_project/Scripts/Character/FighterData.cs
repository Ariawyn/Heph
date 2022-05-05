using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Character
{
    [System.Serializable] 
    [CreateAssetMenu(fileName = "New Fighter", menuName = "Fighter/Basic")]
    public class FighterData : ScriptableObject
    {
        public string ID;
        public int health;
        public int physicalAttack;
        public int magicalAttack;
        public int physicalDefense;
        public int magicalDefense;
        public int desire;
        public BaseCard[] deck;
    }
}
