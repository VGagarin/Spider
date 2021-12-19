using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    internal sealed class Dealer : MonoBehaviour
    {
        private const int Columns = 10;

        [SerializeField] private float _distanceBetweenColumns = 0.8f;
        [SerializeField] private float _smallVerticalOffset = 0.3f;
        [SerializeField] private float _largeVerticalOffset = 0.5f;
        
        [SerializeField] private CardView _cardView;
        [SerializeField] private Deck _deck;
        [SerializeField] private Transform _waitingStack;

        private List<CardView>[] _gameField;
        private List<CardView> _views;

        private void Start()
        {
            Card[] cards = _deck.CreateDeck();

            CreateCardViews(cards);
            InitializeGameField();
            Deal(transform.position);
        }

        private void CreateCardViews(Card[] cards)
        {
            _views = new List<CardView>();
            
            Transform parent = transform;
            
            foreach (Card card in cards)
            {
                CardView cardView = Instantiate(_cardView, parent);
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
            const int cardsToDeal = 54;
            const int cardsToOpen = 10;

            for (int i = 0; i < cardsToDeal; i++)
            {
                int columnIndex = i % Columns;
                int rowIndex = -(i / Columns);
                
                float xOffset = columnIndex * _distanceBetweenColumns;
                float yOffset = rowIndex * _smallVerticalOffset;
                Vector3 offset = new Vector3(xOffset, yOffset, 0);

                _views[i].transform.position = startPosition + offset;
                _gameField[columnIndex].Add(_views[i]);
                
                if (cardsToDeal - i <= cardsToOpen)
                    _views[i].ShowCard();
            }

            for (int i = cardsToDeal; i < _views.Count; i++)
            {
                _views[i].transform.position = _waitingStack.position;
            }
        }
    }
}