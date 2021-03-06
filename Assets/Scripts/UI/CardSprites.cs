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
        private struct CardValueInfo
        {
            public Value Value;
            public Texture Texture;
        }
        
        [Serializable]
        private struct CardSuitInfo
        {
            public Suit Suit;
            public Color Color;
            public Texture Texture;
        }
        
        [Serializable] // Для иконок королей/вальтов/дам
        private struct CardFigureInfo
        {
            public Value Value;
            public Texture Texture;
        }

        [SerializeField] private Texture _whiteCard;
        [SerializeField] private Texture _cardShirt;
        [SerializeField] private CardValueInfo[] _cardValues;
        [SerializeField] private CardSuitInfo[] _cardSuits;

        public Texture GetWhiteCard() => _whiteCard;
        
        public Texture GetCardShirt() => _cardShirt;
        
        public Texture GetValueTexture(Value value)
        {
            CardValueInfo valueInfo = _cardValues.First(info => info.Value == value);

            return valueInfo.Texture;
        }

        public Texture GetSuitTexture(Suit suit)
        {
            CardSuitInfo suitInfo = _cardSuits.First(info => info.Suit == suit);

            return suitInfo.Texture;
        }
        
        public Color GetSuitColor(Suit suit)
        {
            CardSuitInfo suitInfo = _cardSuits.First(info => info.Suit == suit);

            return suitInfo.Color;
        }
    }
}