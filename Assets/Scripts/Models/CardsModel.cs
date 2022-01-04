using System;
using System.Collections.Generic;
using System.Linq;
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
        public event Action<Card> CardOpened;
        
        public void SetDeck(Deck deck)
        {
            _deck = deck;
            _gameField = new GameField();
            _gameField.AddCardsToWaitingZone(_deck.Cards);
            
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

        public List<Card> GetCardsInWaitingZone()
        {
            return _gameField.GetCardsInWaiting();
        }
        
        private void MoveCard(CardMoveData moveData)
        {
            //TODO инкапсулировать проверку на открытие карты
            if (moveData.SourceZone == CardsZone.Main)
            {
                int sourceColumnId = _gameField.FindColumn(moveData.CardToMove);
                List<Card> sourceColumn = _gameField.GetColumn(sourceColumnId);
                int sourceCardIndex = sourceColumn.IndexOf(moveData.CardToMove);
                if (sourceCardIndex > 0)
                    CardOpened?.Invoke(sourceColumn[sourceCardIndex - 1]);
            }
            
            _gameField.MoveCard(moveData);
            CardMoved?.Invoke(moveData);
            
            //TODO инкапсулировать проверку собранной стопки
            //TODO учитывать, что часть верхних карт может быть скрыта
            if (moveData.TargetZone != CardsZone.Main)
                return;
            
            List<Card> column = _gameField.GetColumn(moveData.CardToMove);
            Value[] values = (Value[])Enum.GetValues(typeof(Value));
            
            bool result = true;
            if (column.Last().Value == Value.Ace && column.Count >= values.Length)
            {
                Suit suit = column.Last().Suit;
                List<Card> potentialEndedSequence = column.GetRange(column.Count - values.Length, values.Length);
                
                for (int i = 0; i < values.Length; i++)
                {
                    Card card = potentialEndedSequence[i];
                    if (card.Suit != suit || card.Value != values[i])
                        result = false;
                }
                
                if (result)
                {
                    float delayBetweenCards = SpiderSettings.DealingSettings.DelayBetweenCardsDeal;
                    potentialEndedSequence.Reverse();
                    MoveCardColumn(potentialEndedSequence, CardsZone.Discard, delay: delayBetweenCards);
                }
            }
        }

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