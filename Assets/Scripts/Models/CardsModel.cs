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

        public void MoveCard(BaseCardMoveData baseMoveData)
        {
            CardMoveData cardMoveData = CreateMoveData(baseMoveData);

            MoveCard(cardMoveData);
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
            if (!isTurnAvailable)
                return false;
            
            int sourceColumnId = _gameField.FindColumn(card);
            List<Card> sourceColumn = _gameField.GetColumn(sourceColumnId);
            if (sourceColumn.Count > 1)
                CardOpened?.Invoke(sourceColumn[sourceColumn.IndexOf(card) - 1]);

            List<Card> cardColumn = GetCardColumn(cardId, sourceColumnId);
            MoveCardColumn(cardColumn, targetColumnId);

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
        
        public CardsZone GetCardZone(int cardId)
        {
            Card card = _deck.GetCardById(cardId);
            return _gameField.GetCardZone(card);
        }

        public List<Card> GetCardsInWaitingZone()
        {
            return _gameField.GetCardsInWaiting();
        }

        private void MoveCardColumn(IEnumerable<Card> column, int targetColumnId)
        {
            foreach (Card card in column)
            {
                MoveCard(new BaseCardMoveData
                {
                    CardId = card.Id,
                    TargetStateIsOpen = true,
                    SourceZone = CardsZone.Main,
                    TargetZone = CardsZone.Main,
                    ColumnId = targetColumnId
                });
            }
        }

        private CardMoveData CreateMoveData(BaseCardMoveData baseMoveData)
        {
            int rowId = _gameField.GetColumnLength(baseMoveData.ColumnId);

            return new CardMoveData
            {
                CardToMove = _deck.GetCardById(baseMoveData.CardId),
                DelayBeforeMove = baseMoveData.DelayBeforeMove,
                TargetPosition = Vector3.up * -rowId * SpiderSettings.DealingSettings.SmallVerticalOffset,
                TargetLayer = rowId,
                SourceZone = baseMoveData.SourceZone,
                TargetZone = baseMoveData.TargetZone,
                ColumnId = baseMoveData.ColumnId,
                TargetParent = ColumnPoints[baseMoveData.ColumnId],
                IsLocalMove = true,
                TargetStateIsOpen = baseMoveData.TargetStateIsOpen
            };
        }
    }
}