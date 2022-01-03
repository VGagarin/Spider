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
        public event Action<CardInputData> CapturedCardUpdated; 
        public event Action<CardInputData> CardReturned; 
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
        
        public void CardReleasedOnPosition(int cardId, Vector3 position)
        {
            
            
            
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

            TurnData turnData = cardsModel.IsTurnAvailable(cardInputData.CardId, cardInputData.ColumnId);

            if (turnData.IsTurnAvailable)
            {
                CardMoveData cardMoveData = new CardMoveData
                {
                    CardToMove = turnData.Card,
                    DelayBeforeMove = 0,
                    TargetPosition = turnData.Position,
                    TargetLayer = turnData.Layer,
                    TargetStateIsOpen = true,
                    TargetZone = CardsZone.Main,
                    ColumnIndex = turnData.TargetColumnId,
                    TargetParent = turnData.Parent,
                    IsLocalMove = true
                };

                cardsModel.MoveCard(cardMoveData);
            }
            else
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