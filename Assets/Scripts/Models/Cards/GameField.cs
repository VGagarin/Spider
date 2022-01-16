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
            if (moveData.SourceZoneType == GameZoneType.Main)
                OpenPreviousCardIfNeeded(moveData.CardToMoveId);

            if (moveData.TargetZoneType == GameZoneType.Main)
                MoveCardToMain(ref moveData);
                
            if (moveData.TargetZoneType == GameZoneType.Discard)
                MoveCardToDiscard(ref moveData);
        }

        public int GetColumnLength(int columnId) => _mainZone[columnId].Count;
        
        public List<Card> GetColumnById(int columnId) => _mainZone[columnId];

        public bool IsTurnAvailable(int cardId, int targetColumnId)
        {
            int sourceColumnId = FindColumn(cardId);

            if (targetColumnId == sourceColumnId)
                return false;

            if (!_mainZone[targetColumnId].Any())
                return true;

            Card targetCard = _mainZone[targetColumnId].Last();
            Card cardToMove = _mainZone[sourceColumnId].Find(card => card.Id == cardId);
            
            return CardSequenceChecker.IsTurnAvailable(cardToMove, targetCard);
        }
        
        public int FindColumn(int cardId)
        {
            for (int index = 0; index < _mainZone.Length; index++)
            {
                List<Card> column = _mainZone[index];
                if (column.Any(card => card.Id == cardId))
                    return index;
            }

            throw new Exception("Column not found");
        }

        public bool CardCanBeCaptured(int cardId)
        {
            List<Card> column = GetColumnByCardId(cardId);
            int cardRow = column.FindIndex(card => card.Id == cardId);

            return CardSequenceChecker.CardCanBeCaptured(column, cardRow);
        }
        
        public GameZoneType GetCardZone(int cardId)
        {
            if (_waitingZone.Any(card => card.Id == cardId))
                return GameZoneType.Waiting;
            if (_discardZone.Any(card => card.Id == cardId))
                return GameZoneType.Discard;

            return GameZoneType.Main;
        }

        public List<Card> GetCardsInWaiting() => _waitingZone;

        public bool HasEndedSequenceCollected(int cardId, out List<Card> potentialEndedSequence)
        {
            potentialEndedSequence = null;
            Card card = FindCardInZone(cardId, GameZoneType.Main);
            if (card.Value != Value.Ace)
                return false;

            List<Card> column = GetColumnByCardId(cardId);
            return CardSequenceChecker.HasEndedSequenceCollected(column, out potentialEndedSequence);
        }
        
        private List<Card> GetColumnByCardId(int cardId) => _mainZone[FindColumn(cardId)];
        
        private void MoveCardToMain(ref CardMoveData moveData)
        {
            Card card = FindCardInZone(moveData.CardToMoveId, moveData.SourceZoneType);

            if (moveData.SourceZoneType == GameZoneType.Waiting)
            {
                _waitingZone.Remove(card);
                OpenCardIfNeeded(ref card);
            }
            
            if (moveData.SourceZoneType == GameZoneType.Main)
                _mainZone[FindColumn(card.Id)].Remove(card);
            
            _mainZone[moveData.ColumnId].Add(card);
        }

        private void MoveCardToDiscard(ref CardMoveData moveData)
        {
            Card card = FindCardInZone(moveData.CardToMoveId, moveData.SourceZoneType);
            
            List<Card> column = GetColumnByCardId(card.Id);
            column.Remove(card);

            moveData.MoveCompleted += () =>
            {
                SetIsOpenToCard(ref card, false);
                _discardZone.Add(card);
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
                return _waitingZone.Find(card => card.Id == cardId);
            if (zoneType == GameZoneType.Discard)
                return _discardZone.Find(card => card.Id == cardId);

            int column = FindColumn(cardId);
            return _mainZone[column].Find(card => card.Id == cardId);
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