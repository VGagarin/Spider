using System;
using DefaultNamespace;
using UnityEngine;

internal class Deck : MonoBehaviour
{
    private const int CardsInDeck = 104;
    
    private Card[] _cards;
    
    private void Awake()
    {
        CreateDeck();

        foreach (Card card in _cards)
        {
            Debug.Log(card.ToString());
        }
    }

    private void CreateDeck()
    {
        _cards = new Card[CardsInDeck];

        int i = 0;
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Value value in Enum.GetValues(typeof(Value)))
            {
                Card card = new Card(value, suit);
                
                _cards[i++] = card;
                _cards[i++] = card;
            }
        }
    }
}
