using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Ability;
using Heph.Scripts.Combat.Card;
using Heph.Scripts.Structures.AbilityQueue;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Heph.Scripts.Character
{
    public class FighterHandler : MonoBehaviour
    {
        public bool isPlayerOwned;

        public string ID;
        
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

        #region Card Variables

        private CardQueue desireCardQueue;
        private DeckHandler _deckHandler;
        [SerializeField] public bool shouldSetupStartingDeck = true;
        [SerializeField] public List<BaseCard> startingDeck;

        #endregion

        #region Setup Functions

        public void Setup(FighterData data)
        {
            ID = data.ID;
            maximumHealth = new Stat(data.health);
            currentHealth = new Stat(data.health);
            physicalAttack = new Stat(data.physicalAttack);
            magicalAttack = new Stat(data.magicalAttack);
            physicalDefense = new Stat(data.physicalDefense);
            magicalDefense = new Stat(data.magicalDefense);
            desire = new Stat(data.desire);

            var tempDeckData = data.deck.ToList();
            _deckHandler = new DeckHandler(this, tempDeckData);
            _deckHandler.ShuffleDeck();

            desireCardQueue = new CardQueue(desire.Value);

            CombatEventsManager.Instance.ActionTick += DesireActionTick;
        }

        #endregion

        public List<BaseCard> GetCardsInHand()
        {
            return _deckHandler.Hand;
        }

        public bool QueueCard(BaseCard card)
        {
            var result = desireCardQueue.Queue(card);
            if (result) _deckHandler.Hand.Remove(card);
            return result;
        }

        public bool RemoveCardFromQueue(BaseCard card)
        {
            var result = desireCardQueue.RemoveFromQueue(card);
            _deckHandler.Hand.Add(card);
            return result;
        }

    public IEnumerator ExecuteTopCard(FighterHandler target)
        {
            // HANDLE GETTING AND ACTIVATING TOP CARD
            if (target == null) yield break;
            if (desireCardQueue == null) yield break;
            if (desireCardQueue.Entries() < 1) yield break;
            
            // START AND WAIT FOR CARD EXECUTION
            var cardToExecute = desireCardQueue.Dequeue();
            var cardExecutionCoroutine = StartCoroutine(cardToExecute.Activate(this, target));
            yield return cardExecutionCoroutine;
            
            // MAKE SURE TO DISCARD CARD
            _deckHandler.Discard.Add(cardToExecute);
            
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

        public void HandleStartTurn()
        {
            Debug.Log("Handling start turn, drawing cards for desire value: " +  desire.Value);

            StartCoroutine(_deckHandler.DrawCardsTimed(desire.Value, 0.2f, 1));
        }

        public void HandleEndTurn()
        {
            _deckHandler.DiscardHand();
        }

        private void HandleDefeat()
        {
            Debug.Log("Fighter defeated!");
            CombatEventsManager.Instance.OnFighterDefeatedAction(ID);
        }
    }
}
