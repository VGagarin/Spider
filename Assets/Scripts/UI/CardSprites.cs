using System;
using System.Linq;
using Game.Model;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "CardSprites", menuName = "Visual/CardSprites")]
    internal sealed class CardSprites : ScriptableObject
    {
        [Serializable]
        private struct CardSpriteInfo
        {
            public Card Card;
            public Sprite Sprite;
        }

        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private CardSpriteInfo[] _cardSprites;

        public Sprite GetDefaultSprite() => _defaultSprite;

        public Sprite GetCardSprite(Card card)
        {
            CardSpriteInfo spriteInfo = _cardSprites.First(info => info.Card.EqualsByValueAndSuit(card));

            return spriteInfo.Sprite;
        }
    }
}