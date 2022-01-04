using System;
using UnityEngine;

namespace Game.Model
{
    [Serializable]
    internal struct Card
    {
        [SerializeField] private Value _value;
        [SerializeField] private Suit _suit;

        public Value Value => _value;
        public Suit Suit => _suit;
        
        public int Id { get; private set; }

        public Card(Value value, Suit suit, int id)
        {
            _value = value;
            _suit = suit;
            Id = id;
        }

        public bool EqualsByValueAndSuit(Card other)
        {
            return Value == other.Value && Suit == other.Suit;
        }

        public override string ToString()
        {
            return $"Suit: {Suit}, Value: {Value}, Id: {Id}";
        }
    }
}