using System;
using System.Linq;
using Game.Model;
using Models;
using Models.Base;
using Models.Cards;
using Models.GameZones;
using UnityEngine;
using ViewModels.Base;

namespace ViewModels
{
    internal class InputViewModel : BaseViewModel
    {
        private readonly InputModel _inputModel;

        public event Action CardCapturedFailed;
        
        public InputViewModel()
        {
            _inputModel = ModelRepository.GetModel<InputModel>();
        }

        public void OnMousePositionUpdated(Vector3 mousePosition)
        {
            _inputModel.SetMousePosition(mousePosition);
        }

        public void OnCardTryCaptured(int cardId, Vector3 position)
        {
            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();

            if (!cardsModel.CardCanBeCaptured(cardId))
            {
                CardCapturedFailed?.Invoke();
                return;
            }

            CardInputData cardInputData = new CardInputData
            {
                CardId = cardId,
                ColumnId = cardsModel.GetCardColumnId(cardId),
                Position = position
            };
            
            _inputModel.SetCapturedCardId(cardInputData);
        }

        public void OnCardReleased(int cardId, Vector3 position)
        {
            int columnIndex = FindColumnIndex(position);

            CardInputData cardInputData = new CardInputData
            {
                CardId = cardId,
                ColumnId = columnIndex,
                Position = position
            };

            _inputModel.SetReleasedCardId(cardInputData);
        }
        
        public void OnClosedCardClicking(int cardId)
        {
            GameZoneType zoneType = ModelRepository.GetModel<CardsModel>().GetCardZone(cardId);

            if (zoneType == GameZoneType.Waiting)
                _inputModel.OnWaitingZoneActivated();
        }

        private int FindColumnIndex(Vector3 position)
        {
            Transform[] columnPositions = ModelRepository.GetModel<GameZonesPointsModel>().MainZonePoints;
            float[] horizontalPositions = columnPositions.Select(columnPosition => columnPosition.position.x).ToArray();

            float halfDistanceBetweenColumns = (horizontalPositions[1] - horizontalPositions[0]) / 2f;
            float horizontalPosition = Mathf.Clamp(position.x, horizontalPositions.First(), horizontalPositions.Last());
            for (int columnId = 0; columnId < columnPositions.Length; columnId++)
            {
                float distanceToColumn = Mathf.Abs(horizontalPosition - horizontalPositions[columnId]);

                if (distanceToColumn <= halfDistanceBetweenColumns)
                    return columnId;
            }

            throw new Exception("ColumnId not found");
        }
    }
}