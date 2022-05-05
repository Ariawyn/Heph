using System.Collections.Generic;
using Heph.Scripts.Combat;
using Heph.Scripts.Combat.Ability;
using Heph.Scripts.Combat.Card;
using UnityEngine;

namespace Heph.Scripts.Structures.AbilityQueue
{
    public class CardQueue
    {
        private readonly int _maxDesire;
        private int _currentDesireCost;
        private List<BaseCard> _cards;
        
        public CardQueue(int maxDesire)
        {
            _cards = new List<BaseCard>();
            _maxDesire = maxDesire;
        }

        public int Entries()
        {
            return _cards.Count;
        }
        
        public bool Queue(BaseCard card)
        {
            if (card.type == CardType.None) return false;
            if (card.desireCost + _currentDesireCost > _maxDesire) return false;

            _cards.Add(card);
            _currentDesireCost += card.desireCost;
            return true;
        }

        public BaseCard Dequeue()
        {
            if (_currentDesireCost <= 0) return null;
            
            var cardToDequeue = _cards[0];
            _cards.Remove(cardToDequeue);
            _currentDesireCost -= cardToDequeue.desireCost;
            return cardToDequeue;
        }

        public bool RemoveFromQueue(BaseCard card)
        {
            return _cards.Remove(card);
        }
    }
}
