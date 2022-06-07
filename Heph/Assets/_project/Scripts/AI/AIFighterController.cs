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
            _currentAvailableCards = _fighter.GetCardsInHand();
            var howManyCardsToAttemptToQueue = _fighter.desire.Value;
            if (howManyCardsToAttemptToQueue > _currentAvailableCards.Count) howManyCardsToAttemptToQueue = _currentAvailableCards.Count;
            for (var i = 0; i < howManyCardsToAttemptToQueue; i++)
            {
                var cardToQueue = SelectCard(_currentAvailableCards);
                if (cardToQueue != null)
                {
                    _fighter.QueueCard(cardToQueue);
                    _currentDesireAmountUsed += cardToQueue.desireCost;
                    _currentAvailableCards = _fighter.GetCardsInHand();
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
