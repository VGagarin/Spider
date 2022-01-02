using System;
using Game;
using Game.DataTypes;

namespace Models
{
    internal class CardsModel : IModel
    {
        private Deck _deck;
        private GameField _gameField;
        
        public event Action<Deck> DeckCreated;
        public event Action<CardMoveData> CardMoved; 

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
    }
}