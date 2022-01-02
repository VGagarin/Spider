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

        public void OnCardCaptured(int cardId)
        {
            _inputModel.SetCapturedCardId(cardId);
        }

        public void OnCardReleased(int cardId)
        {
            _inputModel.SetReleasedCardId(cardId);
        }
    }
}