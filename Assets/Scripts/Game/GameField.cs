using System;
using System.Collections.Generic;
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
            
        }
        
        public void MoveCardFromMainToDiscard(Card card, int moveDataColumnIndex)
        {
            
        }

        private void InitializeMainZone()
        {
            _mainZone = new List<Card>[Columns];

            for (int i = 0; i < _mainZone.Length; i++)
                _mainZone[i] = new List<Card>();
        }
    }
}