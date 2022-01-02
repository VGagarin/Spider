using System;
using System.Linq;
using Game.DataTypes;
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

        [SerializeField] private CardSpriteInfo[] _cardSprites;

        public Sprite GetCardSprite(Card card)
        {
            CardSpriteInfo spriteInfo = _cardSprites.First(info => info.Card.EqualsByValueAndSuit(card));

            return spriteInfo.Sprite;
        }
    }
}