using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Heph.Scripts.Character;
using UnityEngine;
using Random = System.Random;

namespace Heph.Scripts.Combat.Card
{
    public class DeckHandler
    {
        private static readonly Random Rng = new Random();

        private readonly FighterHandler _fighterRef;
        
        public List<BaseCard> Deck;
        public List<BaseCard> Hand;
        public List<BaseCard> Discard;

        public DeckHandler(FighterHandler owner)
        {
            _fighterRef = owner;
            Deck = new List<BaseCard>();
            Hand = new List<BaseCard>();
            Discard = new List<BaseCard>();
        }
        
        
        public DeckHandler(FighterHandler owner, List<BaseCard> deck)
        {
            _fighterRef = owner;
            Deck = deck;
            Hand = new List<BaseCard>();
            Discard = new List<BaseCard>();
        }

        // Allow for deck shuffling
        public void ShuffleDeck()
        {
            ShuffleList(Deck);
        }

        public void DrawTopCard()
        {
            if (Deck.Count + Discard.Count <= 0) return;
            if (Deck.Count <= 0) ReturnDiscard();
            var cardToDraw = Deck.First();
            Hand.Add(cardToDraw);
            Deck.RemoveAt(0);
            CombatEventsManager.Instance.OnDrawAction(_fighterRef.isPlayerOwned, cardToDraw);
        }

        public void DrawCards(int amountToDraw)
        {
            Debug.Log("Drawing cards for fighter handler!");
            // Maybe we just do this the lazy way and call DrawTopCard x amount of times,
            // seems like it could be super easy and not all that horribly performant
            var possibleDrawsAmount = AmountOfPossibleCardsToDraw();
            Debug.Log("Amount of possible draws for fighter handler: " + possibleDrawsAmount);
            if (possibleDrawsAmount <= 0) return;
            if (amountToDraw > possibleDrawsAmount) amountToDraw = possibleDrawsAmount;
            
            for (var i = 0; i < amountToDraw; i++)
            {
                DrawTopCard();
            }

        }

        public IEnumerator DrawCardsTimed(int amountToDraw, float drawTime, float deckShuffleTime)
        {
            Debug.Log("Drawing cards for fighter handler!");
            // Maybe we just do this the lazy way and call DrawTopCard x amount of times,
            // seems like it could be super easy and not all that horribly performant
            var possibleDrawsAmount = AmountOfPossibleCardsToDraw();
            Debug.Log("Amount of possible draws for fighter handler: " + possibleDrawsAmount);
            if (possibleDrawsAmount <= 0) yield break;
            if (amountToDraw > possibleDrawsAmount) amountToDraw = possibleDrawsAmount;
            
            var currentDrawnCards = 0;
            while (currentDrawnCards != amountToDraw)
            {
                DrawTopCard();
                currentDrawnCards++;
                yield return Deck.Count > 0 ? new WaitForSeconds(drawTime) : new WaitForSeconds(deckShuffleTime);
            }
        }

        public int AmountOfPossibleCardsToDraw()
        {
            return Deck.Count + Discard.Count;
        }

        public void ReturnDiscard()
        {
            Deck.AddRange(Discard);
            Discard.Clear();
            ShuffleDeck();
        }

        public void DiscardHand()
        {
            Debug.Log("Discarding hand!");
            Discard.AddRange(Hand);
            Hand.Clear();
        }
        
        public void TotalReset()
        {
            Deck.AddRange(Hand);
            Deck.AddRange(Discard);
            Hand.Clear();
            Discard.Clear();
            ShuffleDeck();
        }

        private static void ShuffleList<T>(IList<T> list)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = Rng.Next(n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }  
        }
    }
}
