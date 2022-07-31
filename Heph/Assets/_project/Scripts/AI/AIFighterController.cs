using System.Collections;
using System.Collections.Generic;
using Heph.Scripts.Character;
using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph
{
    public class AIFighterController
    {
        private FighterHandler _fighter;

        private List<BaseCard> _currentAvailableCards;
        private int _currentDesireAmountUsed;
        
        public AIFighterController(FighterHandler fighterHandler)
        {
            _fighter = fighterHandler;
            CombatEventsManager.Instance.SelectAbilitiesStateEntered += StartSelectingAbilities;
            CombatEventsManager.Instance.ResolveAbilitiesStateEntered += ReactToResolveAbilitiesState;
        }

        private void StartSelectingAbilities()
        {
            // MAKE SURE AI HAS A FULL HAND (WE ARE CHECKING FOR THIS BUT IT SHOULD NOT BE AN ISSUE IN THE FIRST PLACE???)
            _currentAvailableCards = _fighter.GetCardsInHand();
            if (_currentAvailableCards.Count != _fighter.desire.baseValue)
            {
                Debug.Log("For some reason AI fighter didnt have full hand, drawing " + (_fighter.desire.baseValue - _currentAvailableCards.Count) + " cards.");
                _fighter.DrawCards(_fighter.desire.baseValue - _currentAvailableCards.Count);
            }
            Debug.Log("AI has "  +_currentAvailableCards.Count + " cards in hand at the moment");
            var howManyCardsToAttemptToQueue = _fighter.desire.Value;
            if (howManyCardsToAttemptToQueue > _currentAvailableCards.Count) howManyCardsToAttemptToQueue = _currentAvailableCards.Count;
            Debug.Log("AI wants to queue : " + howManyCardsToAttemptToQueue + " cards.");
            for (var i = 0; i < howManyCardsToAttemptToQueue; i++)
            {
                var cardToQueue = SelectCard(_currentAvailableCards);
                if (cardToQueue != null)
                {
                    _fighter.QueueCard(cardToQueue);
                    _currentDesireAmountUsed += cardToQueue.desireCost;
                    //_currentAvailableCards = _fighter.GetCardsInHand(); FOR SOME REASON THIS WAS NOT REMOVING CARDS THAT WERE IN QUEUE AND NOT IN HAND AS IF QUEUEING CARD DOES NOT REMOVE IT???
                    _currentAvailableCards.Remove(cardToQueue);
                }
                else
                {
                    break;
                }

            }
        }

        private BaseCard SelectCard(List<BaseCard> cardsAvailable)
        {
            //TODO: Add actual AI card selection functionality here, probably something we can inherit for each specific AI
            //or at least have some variables to control how a enemy AI selects from the available cards.
            if (cardsAvailable.Count == 0) return null;
            return cardsAvailable[0].desireCost + _currentDesireAmountUsed > _fighter.desire.Value ? null : cardsAvailable[0];
        }

        private void ReactToResolveAbilitiesState()
        {
            _currentAvailableCards.Clear();
            _currentDesireAmountUsed = 0;
        }
    }
}
