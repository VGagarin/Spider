using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Model;
using Game.Settings;

namespace Models.Cards
{
    internal class GameField
    {
        private MainGameZone _mainZone = new MainGameZone();
        private WaitingGameZone _waitingZone = new WaitingGameZone();
        private DiscardGameZone _discardZone = new DiscardGameZone();

        private Action<Card> _cardStateChanged;

        public GameField(Action<Card> cardStateChanged, IEnumerable<Card> cardsInWaiting)
        {
            _waitingZone.AddRange(cardsInWaiting);
            
            _cardStateChanged += cardStateChanged;
        }

        public void MoveCard(ref CardMoveData moveData)
        {
            if (moveData.SourceZoneType == GameZoneType.Main)
                OpenPreviousCardIfNeeded(moveData.CardToMoveId);

            if (moveData.TargetZoneType == GameZoneType.Main)
                MoveCardToMain(ref moveData);
                
            if (moveData.TargetZoneType == GameZoneType.Discard)
                MoveCardToDiscard(ref moveData);
        }

        public int GetColumnLength(int columnId) => _mainZone.ColumnLength(columnId);
        
        public List<Card> GetColumnById(int columnId) => _mainZone.GetColumnById(columnId);

        public bool IsTurnAvailable(int cardId, int targetColumnId)
        {
            int sourceColumnId = _mainZone.FindColumn(cardId);

            if (targetColumnId == sourceColumnId)
                return false;

            if (!_mainZone.GetColumnById(targetColumnId).Any())
                return true;

            Card targetCard = _mainZone.GetColumnById(targetColumnId).Last();
            Card cardToMove = _mainZone.GetColumnById(sourceColumnId).Find(card => card.Id == cardId);
            
            return CardSequenceChecker.IsTurnAvailable(cardToMove, targetCard);
        }

        public bool CardCanBeCaptured(int cardId)
        {
            List<Card> column = GetColumnByCardId(cardId);
            int cardRow = column.FindIndex(card => card.Id == cardId);

            return CardSequenceChecker.CardCanBeCaptured(column, cardRow);
        }
        
        public GameZoneType GetCardZone(int cardId)
        {
            if (_waitingZone.IsCardExists(cardId))
                return GameZoneType.Waiting;
            if (_discardZone.IsCardExists(cardId))
                return GameZoneType.Discard;

            return GameZoneType.Main;
        }

        public List<Card> GetCardsInWaiting() => _waitingZone.Cards;

        public int FindColumn(int cardId) => _mainZone.FindColumn(cardId);

        public bool HasEndedSequenceCollected(int cardId, out List<Card> potentialEndedSequence)
        {
            potentialEndedSequence = null;
            Card card = FindCardInZone(cardId, GameZoneType.Main);
            if (card.Value != Value.Ace)
                return false;

            List<Card> column = GetColumnByCardId(cardId);
            return CardSequenceChecker.HasEndedSequenceCollected(column, out potentialEndedSequence);
        }
        
        private List<Card> GetColumnByCardId(int cardId) => _mainZone.GetColumnById(_mainZone.FindColumn(cardId));
        
        private void MoveCardToMain(ref CardMoveData moveData)
        {
            Card card = FindCardInZone(moveData.CardToMoveId, moveData.SourceZoneType);

            if (moveData.SourceZoneType == GameZoneType.Waiting)
            {
                _waitingZone.RemoveCard(card);
                OpenCardIfNeeded(ref card);
            }
            
            if (moveData.SourceZoneType == GameZoneType.Main)
                _mainZone.RemoveCard(card);
            
            _mainZone.AddCard(card, moveData.ColumnId);
        }

        private void MoveCardToDiscard(ref CardMoveData moveData)
        {
            Card card = FindCardInZone(moveData.CardToMoveId, moveData.SourceZoneType);
            
            _mainZone.RemoveCard(card);

            moveData.MoveCompleted += () =>
            {
                SetIsOpenToCard(ref card, false);
                _discardZone.AddCard(card);
            };
        }
        
        private void OpenPreviousCardIfNeeded(int cardId)
        {
            int sourceColumnId = FindColumn(cardId);
            List<Card> sourceColumn = GetColumnById(sourceColumnId);
            int sourceCardIndex = sourceColumn.FindIndex(card => card.Id == cardId);
            if (sourceCardIndex == 0 || sourceColumn[sourceCardIndex - 1].IsOpen) 
                return;
            
            Card card = sourceColumn[sourceCardIndex - 1];
            SetIsOpenToCard(ref card, true);
            sourceColumn[sourceCardIndex - 1] = card;
        }
        
        private void OpenCardIfNeeded(ref Card card)
        {
            int cardsInWaiting = _waitingZone.Count;

            GameRules rules = SpiderSettings.GameRules;
            if (cardsInWaiting < rules.CardsInDeck - rules.CardsToDeal + rules.CardsToOpen)
                SetIsOpenToCard(ref card, true);
        }

        private void SetIsOpenToCard(ref Card card, bool isOpen)
        {
            card.IsOpen = isOpen;
            _cardStateChanged?.Invoke(card);
        }

        private Card FindCardInZone(int cardId, GameZoneType zoneType)
        {
            if (zoneType == GameZoneType.Waiting)
                return _waitingZone.FindCard(cardId);
            if (zoneType == GameZoneType.Discard)
                return _discardZone.FindCard(cardId);

            return _mainZone.FindCard(cardId);
        }

        ~GameField() => _cardStateChanged = null;
    }
}