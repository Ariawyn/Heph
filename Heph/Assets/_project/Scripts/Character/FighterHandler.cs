using Heph.Scripts.Combat;
using UnityEngine;

namespace Heph.Scripts.Character
{
    public class FighterHandler : MonoBehaviour
    {
        #region Stat Variables

        // Health
        // maybe having these as Stats is a bit much?? Its not like we are gonna need modifiers for them... I think
        // also had to make a set function just for this functionality in Stat though that's not like bad.
        public Stat maximumHealth;
        public Stat currentHealth;

        // The basics
        public Stat physicalAttack;
        public Stat magicalAttack;
        public Stat physicalDefense;
        public Stat magicalDefense;

        // desire here acting somewhat like action points
        public Stat desire;

        #endregion

        
        public Ability[] desireAbilityQueue;
        public int currentAbilityQueueingIndex = 0;
        public int currentTotalDesireCost;
        
        [SerializeField] private Ability fillerAbility;

        public void SetupDesireAbilityQueue()
        {
            desireAbilityQueue = new Ability[desire.Value];
        }

        public bool QueueAbility(Ability ability)
        {
            if (currentAbilityQueueingIndex >= desire.Value) return false;
            if (ability.type == AbilityType.None) return false;
            if (currentTotalDesireCost + ability.desireCost > desire.Value) return false;
            
            desireAbilityQueue[currentAbilityQueueingIndex] = ability;
            currentAbilityQueueingIndex++;
            if (ability.desireCost <= 1) return true;
            
            var fillerSpacesToAdd = ability.desireCost - 1;
            for (var i = 0; i < fillerSpacesToAdd; i++)
            {
                desireAbilityQueue[currentAbilityQueueingIndex + i] = fillerAbility;
                currentAbilityQueueingIndex++;  // UGH MAYBE WITH A DATA STRUCTURE FOR DESIRE QUEUE WE WONT HAVE TO DO THIS??? TODO: MAYBE??
                                                // KINDA MAYBE HAS TO BE ITS OWN DATA STRUCTURE SINCE WE DONT ASSOCIATE ONE ABILITY WITH MANY INDEXES CURRENTLY...
                                                // SO LIKE REMOVING AN ABILITY FROM THE QUEUE WOULD NEED TO CHECK DESIRE COST AND REMOVE THE TWO THINGS AFTER
                                                // THEN SORT??? IN DATA STRUCT MAYBE EASIER
            }
            return true;
        }

        public void HandleDamage(int damageAmount, bool physical)
        {
            var totalDamageAmount = damageAmount - (physical ? physicalDefense.Value : magicalDefense.Value);
            if ((currentHealth.Value - totalDamageAmount) > 0)
            {
                currentHealth.Value -= totalDamageAmount;
            }
            else
            {
                HandleDefeat();
            }
        }

        private void HandleDefeat()
        {
            
        }
    }
}
