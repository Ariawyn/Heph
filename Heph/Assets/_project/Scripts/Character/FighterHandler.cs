using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Ability;
using Heph.Scripts.Combat.Card;
using Heph.Scripts.Structures.AbilityQueue;
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

        // Desire here acting like action points per round of combat
        public Stat desire;
        
        // Is the fighter currently paying desire costs
        public bool isBusyWithDesire = false;
        private int _currentDesireActionIndex = 0;
        private int _currentDesireActionRequiredAmount = 0;
        
        
        #endregion

        private CardQueue desireCardQueue;
        public List<BaseCard> deck;

        public void SetupForCombat()
        {
            SetupDesireCardQueue();
            CombatEventsManager.Instance.ActionTick += DesireActionTick;
        }
        

        private void SetupDesireCardQueue()
        {
            desireCardQueue = new CardQueue(desire.Value);
        }

        public bool QueueCard(BaseCard card)
        {
            return desireCardQueue.Queue(card);
        }

        public IEnumerator ExecuteTopCard(FighterHandler target)
        {
            // HANDLE GETTING AND ACTIVATING TOP CARD
            if (desireCardQueue == null) yield break;
            if (desireCardQueue.Entries() < 1) yield break;
            
            var cardToExecute = desireCardQueue.Dequeue();
            StartCoroutine(cardToExecute.Activate(this, target));

            // HANDLE CARD ACTION COST
            if (cardToExecute.desireCost <= 1) yield break;
            isBusyWithDesire = true;
            _currentDesireActionIndex++;
            _currentDesireActionRequiredAmount = cardToExecute.desireCost;
        }

        private void DesireActionTick()
        {
            if (_currentDesireActionIndex + 1 >= _currentDesireActionRequiredAmount)
            {
                isBusyWithDesire = false;
                _currentDesireActionIndex = 0;
                _currentDesireActionRequiredAmount = 0;
            }
            else
            {
                _currentDesireActionIndex++;
            }
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

        public List<BaseCard> DrawCards()
        {
            if (deck.Count <= 0) return null;
            Debug.Log("Deck wasnt null");
            
            List<BaseCard> drawnCards = new List<BaseCard>();
            
            for (var i = 0; i < desire.Value; i++)
            {
                // TODO: Make this actually random
                drawnCards.Add(deck[0]);
            }

            return drawnCards;
        }

        private void HandleDefeat()
        {
            
        }
    }
}
