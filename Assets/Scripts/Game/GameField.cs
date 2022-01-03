﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;

namespace Game
{
    internal class GameField
    {
        private const int Columns = 10;
        
        private List<Card>[] _mainZone;
        private List<Card> _waitingZone = new List<Card>();
        private List<Card> _discardZone = new List<Card>();

        public GameField()
        {
            InitializeMainZone();
        }

        public void AddCardToMainZone(Card card, int column) => _mainZone[column].Add(card);

        public void AddCardsToWaitingZone(IEnumerable<Card> card) => _waitingZone.AddRange(card);

        public void MoveCard(CardMoveData moveData)
        {
            if (moveData.TargetZone == CardsZone.Main)
                MoveCardToMain(moveData);
            if (moveData.TargetZone == CardsZone.Discard)
                MoveCardToDiscard(moveData);
        }

        private void MoveCardToMain(CardMoveData moveData)
        {
            Card card = moveData.CardToMove;
            
            if (moveData.SourceZone == CardsZone.Waiting)
                _waitingZone.Remove(card);
            if (moveData.SourceZone == CardsZone.Main)
                _mainZone[FindColumn(card)].Remove(card);
            
            _mainZone[moveData.ColumnId].Add(card);
        }

        private void MoveCardToDiscard(CardMoveData moveData)
        {
            
        }

        public int GetColumnLength(int columnId) => _mainZone[columnId].Count;
        
        public List<Card> GetColumn(int columnId) => _mainZone[columnId];
        
        public bool IsTurnAvailable(Card card, int targetColumnIndex)
        {
            int sourceColumnId = FindColumn(card);

            if (targetColumnIndex == sourceColumnId)
                return false;

            if (IsColumnEmpty(targetColumnIndex))
                return true;

            Card targetCard = GetUpperCardInColumn(targetColumnIndex);
            return card.Value == targetCard.Value + 1;
        }
        
        public int FindColumn(Card card)
        {
            for (int index = 0; index < _mainZone.Length; index++)
            {
                List<Card> column = _mainZone[index];

                if (column.Contains(card))
                    return index;
            }

            throw new Exception("Column not found");
        }

        private void InitializeMainZone()
        {
            _mainZone = new List<Card>[Columns];

            for (int i = 0; i < _mainZone.Length; i++)
                _mainZone[i] = new List<Card>();
        }

        private bool IsColumnEmpty(int columnId) => !_mainZone[columnId].Any();

        private Card GetUpperCardInColumn(int columnId) => _mainZone[columnId].Last();
    }
}