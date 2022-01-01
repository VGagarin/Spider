using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Game;
using UnityEngine;
using Views;

namespace Spider
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
        
        [SerializeField] private CardView _cardView;
        [SerializeField] private Transform _waitingStack;

        private List<CardView>[] _gameField;
        private List<CardView> _views;

        private CancellationTokenSource _cancellationTokenSource;

        private void Start()
        {
            Card[] cards = Deck.CreateDeck();

            CreateCardViews(cards);
            InitializeGameField();
            Deal(transform.position);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }

        private void CreateCardViews(Card[] cards)
        {
            _views = new List<CardView>();
            
            Transform parent = transform;
            
            foreach (Card card in cards)
            {
                Debug.Log("строчка до инстанса");
                CardView cardView = Instantiate(_cardView, parent);
                Debug.Log("строчка после инстанса");
                cardView.SetCard(card);
                
                _views.Add(cardView);
            }
        }

        private void InitializeGameField()
        {
            _gameField = new List<CardView>[10];

            for (int i = 0; i < _gameField.Length; i++)
                _gameField[i] = new List<CardView>();
        }

        private void Deal(Vector3 startPosition)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            foreach (CardView view in _views)
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