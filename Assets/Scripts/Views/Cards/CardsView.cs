using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private CardMover _cardMover;
        private Camera _mainCamera;
        private CardsDragger _cardsAttacher;

        protected override void Initialize()
        {
            _mainCamera = Camera.main;

            DealingSettings settings = SpiderSettings.DealingSettings;
            _cardMover = new CardMover(settings.CardSpeed, settings.Easing);
            
            _cardsAttacher = new CardsDragger(_mainCamera, _cardMover);
            
            _viewModel.DeckCreated += OnDeckCreated;
            _viewModel.CardMoved += OnCardMoved;
            _viewModel.CardOpened += OnCardOpened;

            _viewModel.CapturedCardsUpdated += OnCapturedCardsUpdated;
            _viewModel.CardReturned += OnCardReturn;
            _viewModel.MousePositionUpdated += OnMousePositionUpdated;
        }

        private void OnDestroy()
        {
            _viewModel.DeckCreated -= OnDeckCreated;
            _viewModel.CardMoved -= OnCardMoved;
            _viewModel.CardOpened -= OnCardOpened;
            
            _viewModel.CapturedCardsUpdated -= OnCapturedCardsUpdated;
            _viewModel.CardReturned -= OnCardReturn;
            _viewModel.MousePositionUpdated -= OnMousePositionUpdated;
        }

        private void OnDeckCreated(Deck deck) => CreateCardSubviews(deck.Cards);
        
        private void OnCardMoved(CardMoveData moveData)
        {
            CardSubview card = _cardSubviewById[moveData.CardToMove.Id];

            _cardsAttacher.Clear();
            
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
        
        private void OnCapturedCardsUpdated(List<Card> cards)
        {
            CardSubview[] attachedCards = cards.Select(card => _cardSubviewById[card.Id]).ToArray();
            Array.ForEach(attachedCards, _cardsAttacher.AttachSubview);
        }

        private void OnCardReturn(CardInputData cardInputData) => _cardsAttacher.ReturnAllAttaches();

        private void OnMousePositionUpdated(Vector3 mousePosition) => _cardsAttacher.UpdatePositions(mousePosition);

        private void CreateCardSubviews(Card[] cards)
        {
            Transform parent = transform;

            for (int i = 0; i < cards.Length; i++)
            {
                CardSubview cardSubview = Instantiate(_cardSubview, _createPoint.position, Quaternion.identity, parent);
                cardSubview.SetCard(cards[i]);

                _cardSubviewById[cards[i].Id] = cardSubview;

                int cardsToFirstDeal = SpiderSettings.GameRules.CardsToDeal;
                cardSubview.Layer = i <= cardsToFirstDeal ? cardsToFirstDeal - i : -i;
            }
        }
    }
}