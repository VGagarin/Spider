using UnityEngine;

namespace DefaultNamespace
{
    internal sealed class CardView : MonoBehaviour
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