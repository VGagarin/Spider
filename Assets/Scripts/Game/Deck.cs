using System;
using System.Linq;
using Spider;
using Random = UnityEngine.Random;

namespace Game
{
    internal static class Deck
    {
        private const int CardsInDeck = 104;
    
        public static Card[] CreateDeck()
        {
            Card[] cards = new Card[CardsInDeck];

            int i = 0;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Value value in Enum.GetValues(typeof(Value)))
                {
                    Card card = new Card(value, suit);
                
                    cards[i++] = card;
                    cards[i++] = card;
                }
            }

            Shuffle(ref cards);

            return cards;
        }

        private static void Shuffle(ref Card[] cards) => cards = cards.OrderBy(card => Random.value).ToArray();
    }
}
