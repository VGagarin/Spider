using System;
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
        
        public Transform[] ColumnPoints { get; private set; }
        
        public Deck Deck => _deck;

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

            MoveCard(new CardMoveData
            {
                CardToMove = card,
                DelayBeforeMove = 0,
                TargetPosition = Vector3.up * -rowIndex * SpiderSettings.DealingSettings.SmallVerticalOffset,
                TargetLayer = rowIndex,
                TargetStateIsOpen = true,
                TargetZone = CardsZone.Main,
                ColumnId = targetColumnId,
                TargetParent = ColumnPoints[targetColumnId],
                IsLocalMove = true
            });

            return true;
        }
    }
}