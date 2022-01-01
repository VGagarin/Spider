using System;
using Game.DataTypes;
using UI;
using UnityEngine;
using ViewModels;

namespace Views
{
    internal sealed class CardView : BaseView<CardViewModel>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CardSprites _cardSprites;

        private Card _card;
        private int _layer;

        public int Layer => _layer;

        public void SetCard(Card card)
        {
            _card = card;
        }

        public void SetLayer(int layer)
        {
            _spriteRenderer.sortingOrder = layer;
            _layer = layer;
        }

        public void ShowCard()
        {
            Sprite sprite = _cardSprites.GetCardSprite(_card);
            
            _spriteRenderer.sprite = sprite;
        }
    }
}