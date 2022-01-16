using System.Collections.Generic;
using System.Linq;
using Game.Model;

namespace Models.Cards
{
    internal abstract class SimpleGameZone : IGameZone
    {
        private List<Card> _cards = new List<Card>();
        
        public abstract GameZoneType ZoneType { get; }
        
        public List<Card> Cards => _cards;
        public int Count => _cards.Count;

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public void AddRange(IEnumerable<Card> cards)
        {
            _cards.AddRange(cards);
        }

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
        }

        public bool IsCardExists(int cardId)
        {
            return _cards.Any(card => card.Id == cardId);
        }

        public Card FindCard(int cardId)
        {
            return _cards.Find(card => card.Id == cardId);
        }
    }
}