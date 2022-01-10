using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Game.Model;
using Game.Settings;
using Random = UnityEngine.Random;

namespace Game
{
    internal class Deck
    {
        private static void Shuffle(ref Card[] cards) => cards = cards.OrderBy(card => Random.value).ToArray();

        private readonly Dictionary<int, Card> _cardById = new Dictionary<int, Card>();
        
        private Card[] _cards;

        public Card[] Cards => _cards;

        public Deck(List<Suit> suits, bool needShuffle = true)
        {
            CreateDeck(suits);
            
            if (needShuffle)
                Shuffle(ref _cards);
        }
        
        private void CreateDeck(List<Suit> suits)
        {
            _cards = new Card[SpiderSettings.GameRules.CardsInDeck];
            
            using IEnumerator<Suit> loopedIncludedValues = suits.ToArray().LoopedIncludedValues();

            int i = 0;
            while (i < SpiderSettings.GameRules.CardsInDeck)
            {
                loopedIncludedValues.MoveNext();
                Suit suit = loopedIncludedValues.Current;
                
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    _cards[i++] = CreateCard(value, suit, i);
                }
            }
        }

        private Card CreateCard(Value value, Suit suit, int id)
        {
            Card card = new Card(value, suit, id);
            _cardById[id] = card;
            return card;
        }
    }
}
