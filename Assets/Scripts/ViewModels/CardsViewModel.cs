using System;
using Game;
using Game.Model;
using Models;
using Models.Base;
using UnityEngine;
using ViewModels.Base;

namespace ViewModels
{
    internal class CardsViewModel : BaseViewModel
    {
        public event Action<Deck> DeckCreated;
        public event Action<CardMoveData> CardMoved;
        public event Action<int> CapturedCardUpdated; 
        public event Action<int> ReleasedCardUpdated; 
        public event Action<Vector3> MousePositionUpdated;

        public CardsViewModel()
        {
            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();
            InputModel inputModel = ModelRepository.GetModel<InputModel>();

            cardsModel.DeckCreated += OnDeckCreated;
            cardsModel.CardMoved += OnCardMoved;

            inputModel.CapturedCardUpdated += OnCapturedCardUpdated;
            inputModel.ReleasedCardUpdated += OnReleasedCardUpdated;
            inputModel.MousePositionUpdated += OnMousePositionUpdated;
        }

        private void OnDeckCreated(Deck deck)
        {
            DeckCreated?.Invoke(deck);
        }
        
        private void OnCardMoved(CardMoveData cardMoveData)
        {
            CardMoved?.Invoke(cardMoveData);
        }
        
        private void OnCapturedCardUpdated(int cardId)
        {
            CapturedCardUpdated?.Invoke(cardId);
        }

        private void OnReleasedCardUpdated(int cardId)
        {
            ReleasedCardUpdated?.Invoke(cardId);
        }
        
        private void OnMousePositionUpdated(Vector3 position)
        {
            MousePositionUpdated?.Invoke(position);
        }

        ~CardsViewModel()
        {
            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();
            InputModel inputModel = ModelRepository.GetModel<InputModel>();
            
            cardsModel.DeckCreated -= OnDeckCreated;
            cardsModel.CardMoved -= OnCardMoved;
            
            inputModel.CapturedCardUpdated -= OnCapturedCardUpdated;
            inputModel.ReleasedCardUpdated -= OnReleasedCardUpdated;
            inputModel.MousePositionUpdated -= OnMousePositionUpdated;
        }
    }
}