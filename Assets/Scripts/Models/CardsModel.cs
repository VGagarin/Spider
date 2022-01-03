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
            int rowIndex = _gameField.GetColumnLength(targetColumnId);
            
            if (!isTurnAvailable)
                return false;
            
            int sourceColumnId = _gameField.FindColumn(card);
            List<Card> sourceColumn = _gameField.GetColumn(sourceColumnId);
            if (sourceColumn.Count > 1)
                CardOpened?.Invoke(sourceColumn[sourceColumn.Count - 2]);

            MoveCard(new CardMoveData
            {
                CardToMove = card,
                TargetPosition = Vector3.up * -rowIndex * SpiderSettings.DealingSettings.SmallVerticalOffset,
                TargetLayer = rowIndex,
                TargetStateIsOpen = true,
                SourceZone = CardsZone.Main,
                TargetZone = CardsZone.Main,
                ColumnId = targetColumnId,
                TargetParent = ColumnPoints[targetColumnId],
                IsLocalMove = true
            });

            return true;
        }
        
        public int GetCardColumnId(int cardId) => _gameField.FindColumn(_deck.GetCardById(cardId));

        public bool CardCanBeCaptured(int cardId) => _gameField.CardCanBeCaptured(_deck.GetCardById(cardId));
    }
}