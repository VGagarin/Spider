using System;
using Game;
using Game.Model;
using Models;
using Models.Base;
using ViewModels.Base;

namespace ViewModels
{
    internal class CardsViewModel : BaseViewModel
    {
        public event Action<Deck> DeckCreated;
        public event Action<CardMoveData> CardMoved; 

        public CardsViewModel()
        {
            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();
            
            cardsModel.DeckCreated += OnDeckCreated;
            cardsModel.CardMoved += OnCardMoved;
        }

        private void OnDeckCreated(Deck deck)
        {
            DeckCreated?.Invoke(deck);
        }
        
        private void OnCardMoved(CardMoveData cardMoveData)
        {
            CardMoved?.Invoke(cardMoveData);
        }

        ~CardsViewModel()
        {
            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();
            
            cardsModel.DeckCreated -= OnDeckCreated;
            cardsModel.CardMoved -= OnCardMoved;
        }
    }
}