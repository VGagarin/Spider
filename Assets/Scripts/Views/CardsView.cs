using System.Collections.Generic;
using DG.Tweening;
using Game;
using Game.Model;
using GeneralPurpose;
using UnityEngine;
using ViewModels;
using Views.Base;

namespace Views
{
    internal sealed class CardsView : BaseView<CardsViewModel>
    {
        [SerializeField] private CardSubview _cardSubview;
        [SerializeField] private Transform _createPoint;
        [SerializeField] private float _cardSpeed;
        [SerializeField] private Ease _easing;
        
        private readonly Dictionary<int, CardSubview> _cardSubviewById = new Dictionary<int, CardSubview>();
        private List<CardSubview> _views;
        private CardMover _cardMover;

        protected override void Initialize()
        {
            _cardMover = new CardMover(_cardSpeed, _easing);
            
            _viewModel.DeckCreated += OnDeckCreated;
            _viewModel.CardMoved += OnCardMoved;
        }

        private void OnDestroy()
        {
            _viewModel.DeckCreated -= OnDeckCreated;
            _viewModel.CardMoved -= OnCardMoved;
        }

        private void OnDeckCreated(Deck deck)
        {
            CreateCardSubviews(deck.Cards);
        }
        
        private void OnCardMoved(CardMoveData moveData)
        {
            CardSubview card = _cardSubviewById[moveData.CardToMove.Id];

            float delayBeforeMove = moveData.DelayBeforeMove;
            Vector3 targetPosition = moveData.TargetPosition;
            Transform cardTransform = card.transform;
            
            InsertAction insertAction = new InsertAction
            {
                Action = () =>
                {
                    card.SetLayer(moveData.TargetLayer);
                    if (moveData.TargetStateIsOpen)
                        card.ShowCard();
                },
                RelativeTime = 0.3f
            };
            
            _cardMover.MoveToPositionAfterDelay(delayBeforeMove, targetPosition, cardTransform, insertAction);
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
                
                const int cardsToFirstDeal = 54;
                
                cardSubview.SetLayer(i <= cardsToFirstDeal ? cardsToFirstDeal - i : -i);
            }
        }
    }
}