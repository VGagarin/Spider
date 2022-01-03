using System;
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
            switch (moveData.TargetZone)
            {
                case CardsZone.Waiting:
                    throw new Exception($"Некорректная {nameof(CardMoveData.TargetZone)} в {nameof(CardMoveData)}");
                case CardsZone.Main:
                    MoveCardFromWaitingToMain(moveData.CardToMove, moveData.ColumnIndex);
                    break;
                case CardsZone.Discard:
                    MoveCardFromMainToDiscard(moveData.CardToMove, moveData.ColumnIndex);
                    break;
                default:
                    throw new Exception($"Некорректная {nameof(CardMoveData.TargetZone)} в {nameof(CardMoveData)}");
            }
        }

        public void MoveCardFromWaitingToMain(Card card, int column)
        {
            _waitingZone.Remove(card);
            _mainZone[column].Add(card);
        }
        
        public void MoveCardFromMainToDiscard(Card card, int moveDataColumnIndex)
        {
            
        }

        public int GetColumnLength(int columnId) => _mainZone[columnId].Count;
        
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