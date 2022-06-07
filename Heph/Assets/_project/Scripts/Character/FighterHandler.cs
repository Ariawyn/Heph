using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Ability;
using Heph.Scripts.Combat.Card;
using Heph.Scripts.Structures.AbilityQueue;
using Heph.Scripts.Util;
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
        public Stat currentOverhealth;
        public Stat currentShield;
        

        // The basics
        public Stat physicalAttack;
        public Stat magicalAttack;
        public Stat physicalDefense;
        public Stat magicalDefense;

        // Desire here acting like action points per round of combat
        public Stat desire;

        // Combat related vars
        public int currentArenaSpaceIndex = 0;
        
        // Is the fighter currently paying desire costs
        public bool isBusyWithDesire = false;
        private int _currentDesireActionIndex = 0;
        private int _currentDesireActionRequiredAmount = 0;

        // AI Handler
        private AIFighterController AIController;

        #endregion

        #region Card Variables

        private CardQueue desireCardQueue;
        private DeckHandler _deckHandler;
        [SerializeField] public Optional<List<BaseCard>> startingDeck;

        #endregion

        #region Setup Functions

        public void Setup(FighterData data, bool playerOwned)
        {
            ID = data.ID;
            maximumHealth = new Stat(data.health);
            currentHealth = new Stat(data.health);
            currentShield = new Stat(0);
            currentOverhealth = new Stat(0);
            physicalAttack = new Stat(data.physicalAttack);
            magicalAttack = new Stat(data.magicalAttack);
            physicalDefense = new Stat(data.physicalDefense);
            magicalDefense = new Stat(data.magicalDefense);
            desire = new Stat(data.desire);

            var tempDeckData = data.deck.ToList();
            _deckHandler = new DeckHandler(this, tempDeckData);
            _deckHandler.ShuffleDeck();

            desireCardQueue = new CardQueue(desire.Value);

            isPlayerOwned = playerOwned;
            if (!isPlayerOwned)
            {
                AIController = new AIFighterController(this);
            }
            
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

        public void AttemptToStartDialogue()
        {
            CombatEventsManager.Instance.OnFighterDialogueStartAction(isPlayerOwned);
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
            // Handle shields
            var currentDamageAmount = damageAmount;
            if (currentShield.Value >= currentDamageAmount)
            {
                currentShield.Value -= currentDamageAmount;
                CombatEventsManager.Instance.OnFighterShieldDamagedAction(isPlayerOwned, currentDamageAmount);
            }
            else
            {
                currentDamageAmount -= currentShield.Value;
                CombatEventsManager.Instance.OnFighterShieldDamagedAction(isPlayerOwned, currentShield.Value);
                currentShield.Value = 0;
            }
            
            // Handle health damage
            var totalDamageAmount = currentDamageAmount- (physical ? physicalDefense.Value : magicalDefense.Value);
            if (totalDamageAmount <= 0) totalDamageAmount = 0;
            
            if ((currentHealth.Value - totalDamageAmount) > 0)
            {
                currentHealth.Value -= totalDamageAmount;
                CombatEventsManager.Instance.OnFighterDamagedAction(isPlayerOwned, totalDamageAmount);
            }
            else
            {
                currentHealth.Value = 0;
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
            // Remove all overhealth
            currentOverhealth.Value = 0;
            // Remove all shield
            currentShield.Value = 0;
            // Discard hand
            _deckHandler.DiscardHand();
        }

        private void HandleDefeat()
        {
            Debug.Log("Fighter defeated!");
            CombatEventsManager.Instance.OnFighterDefeatedAction(ID);
        }
    }
}
