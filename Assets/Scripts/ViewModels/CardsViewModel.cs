using System;
using System.Collections.Generic;
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
        public event Action<List<Card>> CapturedCardsUpdated; 
        public event Action<CardInputData> CardReturned; 
        public event Action<Vector3> MousePositionUpdated;

        private CardsModel _cardsModel;

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
            //TODO захватывать колонну карт
            _cardsModel = ModelRepository.GetModel<CardsModel>();
            List<Card> capturedCards = _cardsModel.GetCardColumn(cardInputData.CardId, cardInputData.ColumnId);

            CapturedCardsUpdated?.Invoke(capturedCards);
        }

        private void OnReleasedCardUpdated(CardInputData cardInputData)
        {
            _cardsModel = ModelRepository.GetModel<CardsModel>();

            bool isTurnPerformed = _cardsModel.PerformTurnIfPossible(cardInputData.CardId, cardInputData.ColumnId);

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
            InputModel inputModel = ModelRepository.GetModel<InputModel>();
            
            _cardsModel.DeckCreated -= OnDeckCreated;
            _cardsModel.CardMoved -= OnCardMoved;
            
            inputModel.CapturedCardUpdated -= OnCapturedCardUpdated;
            inputModel.ReleasedCardUpdated -= OnReleasedCardUpdated;
            inputModel.MousePositionUpdated -= OnMousePositionUpdated;
        }
    }
}