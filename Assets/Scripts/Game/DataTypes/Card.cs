using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DataTypes
{
    [Serializable]
    internal struct Card
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
    }
}