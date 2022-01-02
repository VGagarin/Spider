using System;
using IO;
using ViewModels;

namespace Views
{
    internal class InputView : BaseView<InputViewModel>
    {
        private MousePositionProvider _mousePositionProvider;
        private CardsInputProvider _cardsInputProvider;

        protected override void Initialize()
        {
            _mousePositionProvider = gameObject.AddComponent<MousePositionProvider>();
            _cardsInputProvider = gameObject.AddComponent<CardsInputProvider>();

            _mousePositionProvider.MousePositionUpdated += _viewModel.OnMousePositionUpdated;

            _cardsInputProvider.CardCaptured += _viewModel.OnCardCaptured;
            _cardsInputProvider.CardReleased += _viewModel.OnCardReleased;
        }

        private void OnDestroy()
        {
            _mousePositionProvider.MousePositionUpdated -= _viewModel.OnMousePositionUpdated;

            _cardsInputProvider.CardCaptured -= _viewModel.OnCardCaptured;
            _cardsInputProvider.CardReleased -= _viewModel.OnCardReleased;
        }
    }
}