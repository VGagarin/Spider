using Game.Model;
using UI;
using UnityEngine;
using Views.Base;

namespace Views
{
    internal sealed class CardSubview : BaseSubview<CardsView>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CardSprites _cardSprites;

        private Card _card;
        private int _layer;
        private int _cardId;

        public int Layer => _layer;
        public int CardId => _cardId;

        public void SetCard(Card card)
        {
            _card = card;
            _cardId = card.Id;
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