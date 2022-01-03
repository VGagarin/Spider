using System;
using System.Linq;
using Game.Model;
using Models;
using Models.Base;
using UnityEngine;
using ViewModels.Base;

namespace ViewModels
{
    internal class InputViewModel : BaseViewModel
    {
        private readonly InputModel _inputModel;
        
        public InputViewModel()
        {
            _inputModel = ModelRepository.GetModel<InputModel>();
        }

        public void OnMousePositionUpdated(Vector3 mousePosition)
        {
            _inputModel.SetMousePosition(mousePosition);
        }

        public void OnCardCaptured(int cardId, Vector3 position)
        {
            int columnIndex = FindColumnIndex(position);

            CardInputData cardInputData = new CardInputData
            {
                CardId = cardId,
                ColumnId = columnIndex,
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

        private int FindColumnIndex(Vector3 position)
        {
            Transform[] columnPositions = ModelRepository.GetModel<CardsModel>().ColumnPoints;
            float[] horizontalPositions = columnPositions.Select(columnPosition => columnPosition.position.x).ToArray();
            int columnIndex = Array.BinarySearch(horizontalPositions, position.x);
            columnIndex = columnIndex < 0 ? ~columnIndex : columnIndex;

            return columnIndex;
        }
    }
}