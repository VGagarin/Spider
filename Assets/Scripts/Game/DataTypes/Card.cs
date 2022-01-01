using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataTypes
{
    [Serializable]
    internal struct Card : IEqualityComparer<Card>
    {
        [SerializeField] private Value _value;
        [SerializeField] private Suit _suit;

        public Value Value => _value;
        public Suit Suit => _suit;

        public Card(Value value, Suit suit)
        {
            _value = value;
            _suit = suit;
        }

        public override string ToString()
        {
            return $"Suit: {Suit}, Value: {Value}";
        }

        public bool Equals(Card x, Card y)
        {
            return x._value == y._value && x._suit == y._suit;
        }

        public int GetHashCode(Card obj)
        {
            unchecked
            {
                return ((int) obj._value * 397) ^ (int) obj._suit;
            }
        }
    }
}