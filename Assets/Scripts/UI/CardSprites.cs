using System;
using System.Linq;
using Game.DataTypes;
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

        [SerializeField] private CardSpriteInfo[] _cardSprites;

        public Sprite GetCardSprite(Card card)
        {
            CardSpriteInfo spriteInfo = _cardSprites.First(cardSpriteInfo => cardSpriteInfo.Card.Equals(card));

            return spriteInfo.Sprite;
        }
    }
}