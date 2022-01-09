﻿using Game.Model;
using UI;
using UnityEngine;

namespace Views.Cards
{
    internal static class CardShaderChanger
    {
        private const string MainTexture = "_MainTex";
        private const string ValueTexture = "_ValueTex";
        private const string SuitTexture = "_SuitTex";
        private const string FigureTexture = "_FigureTex";
        private const string SuitColor = "_SuitColor";
        private const string FigureTint = "_FigureTint";

        private static readonly int _mainTexture = Shader.PropertyToID(MainTexture);
        private static readonly int _valueTexture = Shader.PropertyToID(ValueTexture);
        private static readonly int _suitTexture = Shader.PropertyToID(SuitTexture);
        private static readonly int _figureTexture = Shader.PropertyToID(FigureTexture);
        private static readonly int _suitColor = Shader.PropertyToID(SuitColor);
        private static readonly int _figureTint = Shader.PropertyToID(FigureTint);

        public static void ShowCard(Card card, SpriteRenderer spriteRenderer, CardSprites cardSprites)
        {
            Material material = spriteRenderer.material;
            
            spriteRenderer.sprite = cardSprites.GetWhiteCard();

            Texture valueTexture = cardSprites.GetValueTexture(card.Value);
            material.SetTexture(_valueTexture, valueTexture);

            Texture suitTexture = cardSprites.GetSuitTexture(card.Suit);
            material.SetTexture(_suitTexture, suitTexture);
            material.SetTexture(_figureTexture, suitTexture);

            Color suitColor = cardSprites.GetSuitColor(card.Suit);
            material.SetColor(_suitColor, suitColor);
            material.SetColor(_figureTint, suitColor);
        }
        
        public static void HideCard(SpriteRenderer spriteRenderer, CardSprites cardSprites)
        {
            Material material = spriteRenderer.material;
            
            spriteRenderer.sprite = cardSprites.GetDefaultSprite();

            material.SetTexture(_valueTexture, new Texture2D(0, 0));
            material.SetTexture(_suitTexture, new Texture2D(0, 0));
            material.SetTexture(_figureTexture, new Texture2D(0, 0));
        }
    }
}