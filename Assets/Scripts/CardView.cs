using UnityEngine;

namespace DefaultNamespace
{
    internal sealed class CardView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CardSprites _cardSprites;

        private Card _card;
        
        public void SetCard(Card card)
        {
            _card = card;
        }

        public void ShowCard()
        {
            Sprite sprite = _cardSprites.GetCardSprite(_card);
            
            _spriteRenderer.sprite = sprite;
        }
    }
}