using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;
using Game.Settings;
using UnityEngine;

namespace Game
{
    internal class GameField
    {
        private List<Card>[] _mainZone;
        private List<Card> _waitingZone = new List<Card>();
        private List<Card> _discardZone = new List<Card>();

        private Action<Card> _cardStateChanged;

        public GameField(Action<Card> cardStateChanged, IEnumerable<Card> cardsInWaiting)
        {
            InitializeMainZone();
            
            _waitingZone.AddRange(cardsInWaiting);
            
            _cardStateChanged += cardStateChanged;
        }

        public void MoveCard(ref CardMoveData moveData)
        {
            if (moveData.SourceZone == CardsZone.Main)
                OpenPreviousCardIfNeeded(moveData.CardToMove);

            if (moveData.TargetZone == CardsZone.Main)
                MoveCardToMain(moveData);
                
            if (moveData.TargetZone == CardsZone.Discard)
                MoveCardToDiscard(ref moveData);
        }

        public int GetColumnLength(int columnId) => _mainZone[columnId].Count;
        
        public List<Card> GetColumn(int columnId) => _mainZone[columnId];

        public bool IsTurnAvailable(Card card, int targetColumnIndex)
        {
            int sourceColumnId = FindColumn(card);

            if (targetColumnIndex == sourceColumnId)
                return false;

            if (!_mainZone[targetColumnIndex].Any())
                return true;

            Card targetCard = _mainZone[targetColumnIndex].Last();
            return CardSequenceChecker.IsTurnAvailable(card, targetCard);
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

        public bool CardCanBeCaptured(Card card)
        {
            List<Card> column = GetColumn(card);
            int cardRow = column.IndexOf(card);

            return CardSequenceChecker.CardCanBeCaptured(column, cardRow);
        }
        
        public CardsZone GetCardZone(Card card)
        {
            if (_waitingZone.Contains(card))
                return CardsZone.Waiting;
            if (_discardZone.Contains(card))
                return CardsZone.Discard;

            return CardsZone.Main;
        }

        public List<Card> GetCardsInWaiting() => _waitingZone;

        public bool HasEndedSequenceCollected(int columnId, out List<Card> potentialEndedSequence)
        {
            List<Card> column = GetColumn(columnId);
            return CardSequenceChecker.HasEndedSequenceCollected(column, out potentialEndedSequence);
        }
        
        private List<Card> GetColumn(Card card) => _mainZone[FindColumn(card)];
        
        private void MoveCardToMain(CardMoveData moveData)
        {
            Card card = moveData.CardToMove;

            if (moveData.SourceZone == CardsZone.Waiting)
                _waitingZone.Remove(card);
            if (moveData.SourceZone == CardsZone.Main)
                _mainZone[FindColumn(card)].Remove(card);
            
            SetIsOpenToCard(ref card, moveData.TargetStateIsOpen);
            _mainZone[moveData.ColumnId].Add(card);
        }

        private void MoveCardToDiscard(ref CardMoveData moveData)
        {
            Card card = moveData.CardToMove;
            
            List<Card> column = GetColumn(card);
            column.Remove(card);

            moveData.MoveCompleted += () =>
            {
                SetIsOpenToCard(ref card, false);
                _discardZone.Add(card);
            };
        }
        
        private void OpenPreviousCardIfNeeded(Card cardToMove)
        {
            int sourceColumnId = FindColumn(cardToMove);
            List<Card> sourceColumn = GetColumn(sourceColumnId);
            int sourceCardIndex = sourceColumn.IndexOf(cardToMove);
            if (sourceCardIndex == 0 || sourceColumn[sourceCardIndex - 1].IsOpen) 
                return;
            
            Card card = sourceColumn[sourceCardIndex - 1];
            SetIsOpenToCard(ref card, true);
            sourceColumn[sourceCardIndex - 1] = card;
        }

        private void SetIsOpenToCard(ref Card card, bool isOpen)
        {
            card.IsOpen = isOpen;
            _cardStateChanged?.Invoke(card);
        }

        private void InitializeMainZone()
        {
            _mainZone = new List<Card>[SpiderSettings.GameRules.Columns];

            for (int i = 0; i < _mainZone.Length; i++)
                _mainZone[i] = new List<Card>();
        }

        ~GameField() => _cardStateChanged = null;
    }
}