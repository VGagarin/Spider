using System;
using DefaultNamespace;
using UnityEngine;

internal class Deck : MonoBehaviour
{
    private const int CardsInDeck = 104;
    
    public Card[] CreateDeck()
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

        return cards;
    }
}
