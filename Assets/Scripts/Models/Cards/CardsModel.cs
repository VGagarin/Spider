using System;
using System.Collections.Generic;
using Game;
using Game.Model;
using Game.Settings;
using Models.Base;

namespace Models.Cards
{
    internal class CardsModel : IModel
    {
        private GameField _gameField;
        
        public event Action<Deck> DeckCreated;
        public event Action<CardMoveData> CardMoved;
        public event Action<Card> CardStateChanged;
        
        public void SetDeck(Deck deck)
        {
            _gameField = new GameField(CardStateChanged, deck.Cards);
            
            DeckCreated?.Invoke(deck);
        }

        public void MoveCard(BaseCardMoveData baseMoveData)
        {
            CardMoveData cardMoveData = CreateMoveData(baseMoveData);

            MoveCard(cardMoveData);
        }

        public bool PerformTurnIfPossible(int cardId, int targetColumnId)
        {
            bool isTurnAvailable = _gameField.IsTurnAvailable(cardId, targetColumnId);
            if (!isTurnAvailable)
                return false;
            
            int sourceColumnId = _gameField.FindColumn(cardId);
            List<Card> cardColumn = GetCardColumn(cardId, sourceColumnId);
            MoveCardColumn(cardColumn, GameZoneType.Main, targetColumnId);

            return true;
        }
        
        public int GetCardColumnId(int cardId) => _gameField.FindColumn(cardId);

        public bool CardCanBeCaptured(int cardId) => _gameField.CardCanBeCaptured(cardId);

        public List<Card> GetCardColumn(int cardId, int columnId)
        {
            List<Card> column = _gameField.GetColumnById(columnId);
            int cardRow = column.FindIndex(card => card.Id == cardId);
            return column.GetRange(cardRow, column.Count - cardRow);
        }
        
        public GameZoneType GetCardZone(int cardId) => _gameField.GetCardZone(cardId);

        public List<Card> GetCardsInWaitingZone() => _gameField.GetCardsInWaiting();

        private void MoveCardColumn(IEnumerable<Card> column, GameZoneType targetZoneType, int columnId = 0, float delay = 0)
        {
            int i = 0;
            foreach (Card card in column)
            {
                MoveCard(new BaseCardMoveData
                {
                    CardId = card.Id,
                    DelayBeforeMove = ++i * delay,
                    SourceZoneType = GameZoneType.Main,
                    TargetZoneType = targetZoneType,
                    ColumnId = columnId
                });
            }
        }
        
        private void MoveCard(CardMoveData moveData)
        {
            if (moveData.TargetZoneType == GameZoneType.Main)
                moveData.MoveCompleted += () => CheckColumnForEndingSequence(moveData.CardToMoveId);
            
            _gameField.MoveCard(ref moveData);
            CardMoved?.Invoke(moveData);
        }

        private void CheckColumnForEndingSequence(int cardId)
        {
            if (_gameField.HasEndedSequenceCollected(cardId, out List<Card> potentialEndedSequence))
            {
                float delayBetweenCards = SpiderSettings.DealingSettings.DelayBetweenCardsDeal;
                potentialEndedSequence.Reverse();
                MoveCardColumn(potentialEndedSequence, GameZoneType.Discard, delay: delayBetweenCards);
            }
        }

        private CardMoveData CreateMoveData(BaseCardMoveData baseMoveData)
        {
            int rowId = _gameField.GetColumnLength(baseMoveData.ColumnId);

            return new CardMoveData
            {
                CardToMoveId = baseMoveData.CardId,
                DelayBeforeMove = baseMoveData.DelayBeforeMove,
                TargetLayer = rowId,
                SourceZoneType = baseMoveData.SourceZoneType,
                TargetZoneType = baseMoveData.TargetZoneType,
                ColumnId = baseMoveData.ColumnId,
                RowId = rowId
            };
        }
    }
}