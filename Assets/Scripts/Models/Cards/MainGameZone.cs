using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;
using Game.Settings;

namespace Models.Cards
{
    internal class MainGameZone : IGameZone
    {
        private List<Card>[] _mainZone;
        
        public GameZoneType ZoneType => GameZoneType.Main;

        public MainGameZone()
        {
            InitializeMainZone();
        }

        public void AddCard(Card card, int columnId) => _mainZone[columnId].Add(card);
        
        public void RemoveCard(Card card)
        {
            int columnId = FindColumn(card.Id);
            RemoveCard(card, columnId);
        }

        public List<Card> GetColumnById(int columnId) => _mainZone[columnId];

        public int ColumnLength(int columnId) => _mainZone[columnId].Count;
        
        public int FindColumn(int cardId)
        {
            for (int index = 0; index < _mainZone.Length; index++)
            {
                List<Card> column = _mainZone[index];
                if (column.Any(card => card.Id == cardId))
                    return index;
            }

            throw new Exception("Column not found");
        }
        
        public Card FindCard(int cardId)
        {
            int columnId = FindColumn(cardId);
            return _mainZone[columnId].Find(card => card.Id == cardId);
        }
        
        private void RemoveCard(Card card, int columnId) => _mainZone[columnId].Remove(card);

        private void InitializeMainZone()
        {
            _mainZone = new List<Card>[SpiderSettings.GameRules.Columns];

            for (int i = 0; i < _mainZone.Length; i++)
                _mainZone[i] = new List<Card>();
        }
    }
}