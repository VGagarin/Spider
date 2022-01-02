using System.Collections.Generic;
using Game.DataTypes;
using UnityEngine;
using ViewModels;

namespace Views
{
    internal sealed class CardsView : BaseView<CardsViewModel>
    {
        [SerializeField] private CardSubview _cardSubview;
        
        private List<CardSubview> _views;
        
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
    }
}