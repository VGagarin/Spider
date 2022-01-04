using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;
using Random = UnityEngine.Random;

namespace Game
{
    internal class Deck
    {
        private static void Shuffle(ref Card[] cards) => cards = cards.OrderBy(card => Random.value).ToArray();
        
        private const int CardsInDeck = 104;

        private readonly Dictionary<int, Card> _cardById = new Dictionary<int, Card>();
        
        private Card[] _cards;

        public Card[] Cards => _cards;

        public Deck()
        {
            CreateDeck();
        }

        public Card GetCardById(int id)
        {
            return _cardById[id];
        }

        private void CreateDeck()
        {
            _cards = new Card[CardsInDeck];

            int i = 0;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    _cards[i++] = CreateCard(value, suit, i);
                    _cards[i++] = CreateCard(value, suit, i);
                }
            }

            Shuffle(ref _cards);
        }

        private Card CreateCard(Value value, Suit suit, int id)
        {
            Card card = new Card(value, suit, id);
            _cardById[id] = card;
            return card;
        }
    }
}
