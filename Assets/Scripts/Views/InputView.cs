using System;
using IO;
using ViewModels;

namespace Views
{
    internal class InputView : BaseView<InputViewModel>
    {
        private MousePositionProvider _mousePositionProvider;
        private CardsInputProvider _cardsInputProvider;

        private void Start()
        {
            _mousePositionProvider = gameObject.AddComponent<MousePositionProvider>();
            _cardsInputProvider = gameObject.AddComponent<CardsInputProvider>();

            _mousePositionProvider.MousePositionUpdated += _viewModel.OnMousePositionUpdated;

            _cardsInputProvider.CardCaptured += _viewModel.OnCardCaptured;
            _cardsInputProvider.CardReleased += _viewModel.OnCardReleased;
        }
    }
}