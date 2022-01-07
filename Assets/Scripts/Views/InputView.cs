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

            _mousePositionProvider.MousePositionUpdated.AddListener(_viewModel.OnMousePositionUpdated);

            _cardsInputProvider.CardTryCaptured.AddListener(_viewModel.OnCardTryCaptured);
            _cardsInputProvider.CardReleased.AddListener(_viewModel.OnCardReleased);
            _cardsInputProvider.ClosedCardClicking.AddListener(_viewModel.OnClosedCardClicking);

            _viewModel.CardCapturedFailed += _cardsInputProvider.CardCapturedFailed;
        }

        private void OnDestroy()
        {
            _mousePositionProvider.MousePositionUpdated.RemoveAllListeners();

            _cardsInputProvider.CardTryCaptured.RemoveAllListeners();
            _cardsInputProvider.CardReleased.RemoveAllListeners();
            _cardsInputProvider.ClosedCardClicking.RemoveAllListeners();
            
            _viewModel.CardCapturedFailed -= _cardsInputProvider.CardCapturedFailed;
        }
    }
}