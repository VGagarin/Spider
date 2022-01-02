using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Game.DataTypes;
using UnityEngine;
using Views;

namespace Game
{
    internal sealed class Dealer : MonoBehaviour
    {
        private const int Columns = 10;

        [SerializeField] private float _distanceBetweenColumns = 0.8f;
        [SerializeField] private float _smallVerticalOffset = 0.3f;
        [SerializeField] private float _largeVerticalOffset = 0.5f;
        [SerializeField] private float _delayBetweenCardsWhenDealing = 0.05f;
        [SerializeField] private float _cardSpeed = 1f;
        [SerializeField] private Ease _easing;
        
        [SerializeField] private CardSubview _cardSubview;
        [SerializeField] private Transform _waitingStack;

        private List<CardSubview>[] _gameField;
        private List<CardSubview> _views;

        private CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            Card[] cards = Deck.CreateDeck();

            CreateCardSubviews(cards);
            InitializeGameField();
            Deal(transform.position);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }

        private void CreateCardSubviews(Card[] cards)
        {
            _views = new List<CardSubview>();
            
            Transform parent = transform;

            int cardId = 0;
            foreach (Card card in cards)
            {
                CardSubview cardSubview = Instantiate(_cardSubview, parent);
                cardSubview.SetCard(card, cardId);
                
                _views.Add(cardSubview);
            }
        }

        private void InitializeGameField()
        {
            _gameField = new List<CardSubview>[10];

            for (int i = 0; i < _gameField.Length; i++)
                _gameField[i] = new List<CardSubview>();
        }

        private void Deal(Vector3 startPosition)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            foreach (CardSubview view in _views)
                view.transform.position = _waitingStack.position;

            const int cardsToDeal = 54;
            const int cardsToOpen = 10;

            for (int i = 0; i < cardsToDeal; i++)
            {
                int columnIndex = i % Columns;
                int rowIndex = i / Columns;
                
                float xOffset = columnIndex * _distanceBetweenColumns;
                float yOffset = -rowIndex * _smallVerticalOffset;
                Vector3 offset = new Vector3(xOffset, yOffset, 0);

                _gameField[columnIndex].Add(_views[i]);
                
                _views[i].SetLayer(rowIndex);

                float delay = _delayBetweenCardsWhenDealing * i;
                MoveToPositionAfterDelay(delay, startPosition + offset, _views[i].transform);
                
                if (cardsToDeal - i <= cardsToOpen)
                    _views[i].ShowCard();
            }
        }

        private async void MoveToPositionAfterDelay(float delay, Vector3 target, Transform card)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(delay), _cancellationTokenSource.Token);

                float duration = Vector3.Distance(target, card.position) / _cardSpeed;

                card
                    .DOMove(target, duration)
                    .SetEase(_easing);
            }
            catch (OperationCanceledException)
            {
                card?.DOKill();
            }
        }
    }
}