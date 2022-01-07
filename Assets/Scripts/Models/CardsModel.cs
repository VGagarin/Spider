using System;
using System.Collections.Generic;
using Game;
using Game.Model;
using Game.Settings;
using Models.Base;

namespace Models
{
    internal class CardsModel : IModel
    {
        private Deck _deck;
        private GameField _gameField;
        
        public event Action<Deck> DeckCreated;
        public event Action<CardMoveData> CardMoved;
        public event Action<Card> CardStateChanged;
        
        public void SetDeck(Deck deck)
        {
            _deck = deck;
            _gameField = new GameField(CardStateChanged, _deck.Cards);
            
            DeckCreated?.Invoke(_deck);
        }

        public void MoveCard(BaseCardMoveData baseMoveData)
        {
            CardMoveData cardMoveData = CreateMoveData(baseMoveData);

            MoveCard(cardMoveData);
        }

        public bool PerformTurnIfPossible(int cardId, int targetColumnId)
        {
            Card card = _deck.GetCardById(cardId);
            
            bool isTurnAvailable = _gameField.IsTurnAvailable(card, targetColumnId);
            if (!isTurnAvailable)
                return false;
            
            int sourceColumnId = _gameField.FindColumn(card);
            List<Card> cardColumn = GetCardColumn(cardId, sourceColumnId);
            MoveCardColumn(cardColumn, CardsZone.Main, targetColumnId);

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

        public List<Card> GetCardsInWaitingZone() => _gameField.GetCardsInWaiting();

        private void MoveCardColumn(IEnumerable<Card> column, CardsZone targetZone, int columnId = 0, float delay = 0)
        {
            int i = 0;
            foreach (Card card in column)
            {
                MoveCard(new BaseCardMoveData
                {
                    CardId = card.Id,
                    DelayBeforeMove = ++i * delay,
                    TargetStateIsOpen = true,
                    SourceZone = CardsZone.Main,
                    TargetZone = targetZone,
                    ColumnId = columnId
                });
            }
        }
        
        private void MoveCard(CardMoveData moveData)
        {
            if (moveData.TargetZone == CardsZone.Main && moveData.CardToMove.Value == Value.Ace)
                moveData.MoveCompleted += () => CheckColumnForEndingSequence(moveData.ColumnId);
            
            _gameField.MoveCard(moveData);
            CardMoved?.Invoke(moveData);
        }

        private void CheckColumnForEndingSequence(int columnId)
        {
            if (_gameField.HasEndedSequenceCollected(columnId, out List<Card> potentialEndedSequence))
            {
                float delayBetweenCards = SpiderSettings.DealingSettings.DelayBetweenCardsDeal;
                potentialEndedSequence.Reverse();
                MoveCardColumn(potentialEndedSequence, CardsZone.Discard, delay: delayBetweenCards);
            }
        }

        private CardMoveData CreateMoveData(BaseCardMoveData baseMoveData)
        {
            int rowId = _gameField.GetColumnLength(baseMoveData.ColumnId);

            return new CardMoveData
            {
                CardToMove = _deck.GetCardById(baseMoveData.CardId),
                DelayBeforeMove = baseMoveData.DelayBeforeMove,
                TargetLayer = rowId,
                SourceZone = baseMoveData.SourceZone,
                TargetZone = baseMoveData.TargetZone,
                ColumnId = baseMoveData.ColumnId,
                RowId = rowId,
                TargetStateIsOpen = baseMoveData.TargetStateIsOpen
            };
        }
    }
}