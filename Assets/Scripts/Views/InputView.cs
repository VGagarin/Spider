using IO;
using ViewModels;
using Views.Base;

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

            _cardsInputProvider.CardTryCaptured += _viewModel.OnCardTryCaptured;
            _cardsInputProvider.CardReleased += _viewModel.OnCardReleased;

            _viewModel.CardCapturedFailed += _cardsInputProvider.CardCapturedFailed;
        }

        private void OnDestroy()
        {
            _mousePositionProvider.MousePositionUpdated -= _viewModel.OnMousePositionUpdated;

            _cardsInputProvider.CardTryCaptured -= _viewModel.OnCardTryCaptured;
            _cardsInputProvider.CardReleased -= _viewModel.OnCardReleased;
            
            _viewModel.CardCapturedFailed -= _cardsInputProvider.CardCapturedFailed;
        }
    }
}