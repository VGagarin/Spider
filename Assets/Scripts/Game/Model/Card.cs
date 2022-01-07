using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Model
{
    [Serializable]
    internal struct Card : IEquatable<Card>, IEqualityComparer<Card>
    {
        [SerializeField] private Value _value;
        [SerializeField] private Suit _suit;

        public Value Value => _value;
        public Suit Suit => _suit;
        
        public bool IsOpen { get; set; }
        public int Id { get; private set; }

        public Card(Value value, Suit suit, int id)
        {
            _value = value;
            _suit = suit;
            Id = id;
            IsOpen = false;
        }

        public void Deconstruct(out Value value, out Suit suit)
        {
            value = _value;
            suit = _suit;
        }

        public bool EqualsByValueAndSuit(Card other)
        {
            return Value == other.Value && Suit == other.Suit;
        }
        
        public bool Equals(Card other)
        {
            return _value == other._value && _suit == other._suit && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is Card other && Equals(other);
        }

        public bool Equals(Card x, Card y) => x.Equals(y);

        public override string ToString()
        {
            return $"Suit: {Suit}, Value: {Value}, Id: {Id}";
        }

        public override int GetHashCode() => GetHashCode(this);

        public int GetHashCode(Card card)
        {
            unchecked
            {
                int hashCode = (int) card._value;
                hashCode = (hashCode * 397) ^ (int) card._suit;
                hashCode = (hashCode * 397) ^ card.Id;
                return hashCode;
            }
        }
    }
}