using System.Collections.Generic;
using Game;
using Game.Model;
using Game.Settings;
using GeneralPurpose;
using UnityEngine;
using ViewModels;
using Views.Base;

namespace Views.Cards
{
    internal sealed class CardsView : BaseView<CardsViewModel>
    {
        [SerializeField] private CardSubview _cardSubview;
        [SerializeField] private Transform _createPoint;
        
        private readonly Dictionary<int, CardSubview> _cardSubviewById = new Dictionary<int, CardSubview>();
        private List<CardSubview> _views;
        private CardMover _cardMover;

        private CardSubview _attachedSubview;
        private Vector3 _offset;
        private Camera _mainCamera;
        private Vector3 _startLocalPosition;

        protected override void Initialize()
        {
            _mainCamera = Camera.main;
            
            DealingSettings settings = SpiderSettings.DealingSettings;
            _cardMover = new CardMover(settings.CardSpeed, settings.Easing);
            
            _viewModel.DeckCreated += OnDeckCreated;
            _viewModel.CardMoved += OnCardMoved;
            _viewModel.CardOpened += OnCardOpened;

            _viewModel.CapturedCardUpdated += OnCapturedCardUpdated;
            _viewModel.CardReturned += OnReturnCard;
            _viewModel.MousePositionUpdated += OnMousePositionUpdated;
        }

        private void OnDestroy()
        {
            _viewModel.DeckCreated -= OnDeckCreated;
            _viewModel.CardMoved -= OnCardMoved;
            _viewModel.CardOpened -= OnCardOpened;
            
            _viewModel.CapturedCardUpdated -= OnCapturedCardUpdated;
            _viewModel.CardReturned -= OnReturnCard;
            _viewModel.MousePositionUpdated -= OnMousePositionUpdated;
        }

        private void OnDeckCreated(Deck deck) => CreateCardSubviews(deck.Cards);
        
        private void OnCardMoved(CardMoveData moveData)
        {
            CardSubview card = _cardSubviewById[moveData.CardToMove.Id];
            
            _attachedSubview = null;

            float delayBeforeMove = moveData.DelayBeforeMove;
            Vector3 targetPosition = moveData.TargetPosition;
            Transform cardTransform = card.transform;
            cardTransform.parent = moveData.TargetParent;
            
            if (moveData.TargetStateIsOpen)
                card.ShowCard();
            
            InsertAction insertAction = new InsertAction
            {
                Action = () => card.Layer = moveData.TargetLayer,
                RelativeTime = 0.3f
            };
            
            _cardMover.MoveToLocalPositionAfterDelay(delayBeforeMove, targetPosition, cardTransform, insertAction);
        }
        
        private void OnCardOpened(Card card) => _cardSubviewById[card.Id].ShowCard();
        
        private void OnCapturedCardUpdated(CardInputData cardInputData)
        {
            if (!_cardSubviewById[cardInputData.CardId].IsMovable)
                return;
            
            _attachedSubview = _cardSubviewById[cardInputData.CardId];
            _attachedSubview.Layer += SpiderSettings.GameRules.CardsInDeck;

            Transform attachedSubviewTransform = _attachedSubview.transform;
            _startLocalPosition = attachedSubviewTransform.localPosition;
            _offset = attachedSubviewTransform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _offset.z = 0;
        }

        private void OnReturnCard(CardInputData cardInputData)
        {
            CardSubview cachedSubview = _attachedSubview;
            InsertAction insertAction = new InsertAction
            {
                Action = () => cachedSubview.Layer -= SpiderSettings.GameRules.CardsInDeck,
                RelativeTime = 1f
            };
            
            _cardMover.MoveToLocalPositionAfterDelay(0, _startLocalPosition, _attachedSubview.transform, insertAction);
            
            _attachedSubview = null;
        }

        private void OnMousePositionUpdated(Vector3 mousePosition)
        {
            if (_attachedSubview)
            {
                Vector3 screenToWorldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                
                Transform attachedTransform = _attachedSubview.transform;
                screenToWorldPoint.z = attachedTransform.position.z;
                attachedTransform.position = screenToWorldPoint + _offset;
            }
        }

        private void CreateCardSubviews(Card[] cards)
        {
            _views = new List<CardSubview>();
            
            Transform parent = transform;

            for (int i = 0; i < cards.Length; i++)
            {
                CardSubview cardSubview = Instantiate(_cardSubview, _createPoint.position, Quaternion.identity, parent);
                cardSubview.SetCard(cards[i]);

                _cardSubviewById[cards[i].Id] = cardSubview;
                _views.Add(cardSubview);

                int cardsToFirstDeal = SpiderSettings.GameRules.CardsToDeal;
                cardSubview.Layer = i <= cardsToFirstDeal ? cardsToFirstDeal - i : -i;
            }
        }
    }
}