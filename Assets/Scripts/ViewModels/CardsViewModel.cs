using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Model;
using Game.Settings;
using Models;
using Models.Base;
using Models.GameZones;
using UnityEngine;
using ViewModels.Base;
using Views.Cards;

namespace ViewModels
{
    internal class CardsViewModel : BaseViewModel
    {
        public event Action<Deck, Transform> DeckCreated;
        public event Action<CardMoveData, CardPositionData> CardMoved;
        public event Action<Card> CardStateChanged;
        public event Action<List<Card>> CapturedCardsUpdated; 
        public event Action<CardInputData> CardReturned; 
        public event Action<Vector3> MousePositionUpdated;

        private CardsModel _cardsModel;
        
        public float DistanceBetweenColumnsInMainZone
        {
            get
            {
                Transform[] mainZonePoints = ModelRepository.GetModel<GameZonesModel>().MainZonePoints;
                return mainZonePoints[1].position.x - mainZonePoints[0].position.x;
            }
        }

        public CardsViewModel()
        {
            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();
            InputModel inputModel = ModelRepository.GetModel<InputModel>();

            cardsModel.DeckCreated += OnDeckCreated;
            cardsModel.CardMoved += OnCardMoved;
            cardsModel.CardStateChanged += OnCardStateChanged;

            inputModel.CapturedCardUpdated += OnCapturedCardUpdated;
            inputModel.ReleasedCardUpdated += OnReleasedCardUpdated;
            inputModel.MousePositionUpdated += OnMousePositionUpdated;
        }

        private void OnCardStateChanged(Card card)
        {
            CardStateChanged?.Invoke(card);
        }

        private void OnDeckCreated(Deck deck)
        {
            WaitingZone waitingZone = ModelRepository.GetModel<GameZonesModel>().WaitingZone;

            DeckCreated?.Invoke(deck, waitingZone.GetPoint());
        }
        
        private void OnCardMoved(CardMoveData cardMoveData)
        {
            IGameZone targetZone = ModelRepository.GetModel<GameZonesModel>().GetZoneByType(cardMoveData.TargetZone);
            Vector3 position = Vector3.zero;
            if (cardMoveData.TargetZone == CardsZone.Main)
            {
                float offset = cardMoveData.RowId * SpiderSettings.GameFieldLayoutSettings.SmallVerticalOffset;
                position = Vector3.down * offset;
            }

            CardPositionData positionData = new CardPositionData
            {
                LocalPosition = position,
                Parent = targetZone.GetPoint(cardMoveData.ColumnId)
            };

            CardMoved?.Invoke(cardMoveData, positionData);
        }
        
        private void OnCapturedCardUpdated(CardInputData cardInputData)
        {
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
            _cardsModel.CardStateChanged -= OnCardStateChanged;
            
            inputModel.CapturedCardUpdated -= OnCapturedCardUpdated;
            inputModel.ReleasedCardUpdated -= OnReleasedCardUpdated;
            inputModel.MousePositionUpdated -= OnMousePositionUpdated;
        }
    }
}