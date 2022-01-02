using Game.Model;
using UI;
using UnityEngine;
using Views.Base;

namespace Views.Cards
{
    internal sealed class CardSubview : BaseSubview<CardsView>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CardSprites _cardSprites;

        private Card _card;
        private int _layer;
        private int _cardId;
        
        public int CardId => _cardId;
        public bool IsMovable { get; set; }
        
        public int Layer
        {
            get => _layer;
            set
            {
                _spriteRenderer.sortingOrder = value;
                _layer = value;
            }
        }

        public void SetCard(Card card)
        {
            _card = card;
            _cardId = card.Id;
        }

        public void ShowCard()
        {
            Sprite sprite = _cardSprites.GetCardSprite(_card);
            
            _spriteRenderer.sprite = sprite;
            IsMovable = true;
        }
    }
}