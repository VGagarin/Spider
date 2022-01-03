using System;
using System.Collections.Generic;
using Game;
using Game.Model;
using Game.Settings;
using Models.Base;
using UnityEngine;

namespace Models
{
    internal class CardsModel : IModel
    {
        private Deck _deck;
        private GameField _gameField;
        
        public event Action<Deck> DeckCreated;
        public event Action<CardMoveData> CardMoved;
        public event Action<Card> CardOpened;
        
        public Transform[] ColumnPoints { get; private set; }
        
        public void CreateDeck()
        {
            _deck = new Deck();
            _gameField = new GameField();
            _gameField.AddCardsToWaitingZone(_deck.Cards);
            
            DeckCreated?.Invoke(_deck);
        }

        public void MoveCard(CardMoveData moveData)
        {
            _gameField.MoveCard(moveData);
            CardMoved?.Invoke(moveData);
        }

        public void UpdateColumnPoints(Transform[] points)
        {
            ColumnPoints = points;
        }

        public bool PerformTurnIfPossible(int cardId, int targetColumnId)
        {
            Card card = _deck.GetCardById(cardId);
            
            bool isTurnAvailable = _gameField.IsTurnAvailable(card, targetColumnId);
            int targetRowIndex = _gameField.GetColumnLength(targetColumnId);
            
            if (!isTurnAvailable)
                return false;
            
            int sourceColumnId = _gameField.FindColumn(card);
            List<Card> sourceColumn = _gameField.GetColumn(sourceColumnId);
            if (sourceColumn.Count > 1)
                CardOpened?.Invoke(sourceColumn[sourceColumn.IndexOf(card) - 1]);

            List<Card> cardColumn = GetCardColumn(cardId, sourceColumnId);
            MoveCardColumn(cardColumn, targetRowIndex, targetColumnId);

            return true;
        }
        
        public int GetCardColumnId(int cardId) => _gameField.FindColumn(_deck.GetCardById(cardId));

        public bool CardCanBeCaptured(int cardId) => _gameField.CardCanBeCaptured(_deck.GetCardById(cardId));

        public List<Card> GetCardColumn(int cardId, int columnId)
        {
            List<Card> column = _gameField.GetColumn(columnId);
            int cardRow = column.IndexOf(_deck.GetCardById(cardId));
            return column.GetRange(cardRow, column.Count - cardRow);
        }

        private void MoveCardColumn(IReadOnlyList<Card> column, int targetRowId, int targetColumnId)
        {
            for (int i = 0; i < column.Count; i++)
            {
                int row = targetRowId + i;

                MoveCard(new CardMoveData
                {
                    CardToMove = column[i],
                    TargetPosition = Vector3.up * -row * SpiderSettings.DealingSettings.SmallVerticalOffset,
                    TargetLayer = row,
                    TargetStateIsOpen = true,
                    SourceZone = CardsZone.Main,
                    TargetZone = CardsZone.Main,
                    ColumnId = targetColumnId,
                    TargetParent = ColumnPoints[targetColumnId],
                    IsLocalMove = true
                });
            }
        }
    }
}