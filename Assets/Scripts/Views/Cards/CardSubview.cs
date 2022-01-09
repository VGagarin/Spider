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
        
        private float HorizontalSize => transform.localScale.x * _spriteRenderer.sprite.bounds.size.x;

        public int Layer
        {
            get => _layer;
            set
            {
                if (_spriteRenderer)
                    _spriteRenderer.sortingOrder = value;
                _layer = value;
            }
        }
        
        public void SetCard(Card card)
        {
            _card = card;
            _cardId = card.Id;
            SetIsOpen(false);
        }

        public void UpdateScaleByHorizontalSize(float targetHorizontalSize)
        {
            float ratio = targetHorizontalSize / HorizontalSize;
            
            Transform cardTransform = transform;
            
            Vector3 localScale = cardTransform.localScale;
            cardTransform.localScale = localScale * ratio;
        }
        
        public void SetIsOpen(bool isOpen)
        {
            if (isOpen)
            {
                CardShaderChanger.ShowCard(_card, _spriteRenderer, _cardSprites);
            }
            else
            {
                CardShaderChanger.HideCard(_spriteRenderer, _cardSprites);
            }

            IsMovable = isOpen;
        }
    }
}