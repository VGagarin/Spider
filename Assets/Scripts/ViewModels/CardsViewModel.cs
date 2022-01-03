using System;
using System.Linq;
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
        public event Action<Card> CardOpened;
        public event Action<CardInputData> CapturedCardUpdated; 
        public event Action<CardInputData> CardReturned; 
        public event Action<Vector3> MousePositionUpdated;

        public CardsViewModel()
        {
            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();
            InputModel inputModel = ModelRepository.GetModel<InputModel>();

            cardsModel.DeckCreated += OnDeckCreated;
            cardsModel.CardMoved += OnCardMoved;
            cardsModel.CardOpened += OnCardOpened;

            inputModel.CapturedCardUpdated += OnCapturedCardUpdated;
            inputModel.ReleasedCardUpdated += OnReleasedCardUpdated;
            inputModel.MousePositionUpdated += OnMousePositionUpdated;
        }

        private void OnCardOpened(Card card)
        {
            CardOpened?.Invoke(card);
        }

        private void OnDeckCreated(Deck deck)
        {
            DeckCreated?.Invoke(deck);
        }
        
        private void OnCardMoved(CardMoveData cardMoveData)
        {
            CardMoved?.Invoke(cardMoveData);
        }
        
        private void OnCapturedCardUpdated(CardInputData cardInputData)
        {
            CapturedCardUpdated?.Invoke(cardInputData);
        }

        private void OnReleasedCardUpdated(CardInputData cardInputData)
        {
            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();

            bool isTurnPerformed = cardsModel.PerformTurnIfPossible(cardInputData.CardId, cardInputData.ColumnId);

            if (!isTurnPerformed)
            {
                CardReturned?.Invoke(cardInputData);
            }
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