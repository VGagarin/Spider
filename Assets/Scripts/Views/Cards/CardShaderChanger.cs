using Game.Model;
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

        private static readonly Texture2D _emptyTexture;
        
        public static void ShowCard(Card card, Material material, CardSprites cardSprites)
        {
            Texture whiteCard = cardSprites.GetWhiteCard();
            material.SetTexture(_mainTexture, whiteCard);

            Texture valueTexture = cardSprites.GetValueTexture(card.Value);
            material.SetTexture(_valueTexture, valueTexture);

            Texture suitTexture = cardSprites.GetSuitTexture(card.Suit);
            material.SetTexture(_suitTexture, suitTexture);
            material.SetTexture(_figureTexture, suitTexture);

            Color suitColor = cardSprites.GetSuitColor(card.Suit);
            material.SetColor(_suitColor, suitColor);
            material.SetColor(_figureTint, suitColor);
        }
        
        public static void HideCard(Material material, CardSprites cardSprites)
        {
            Texture cardShirt = cardSprites.GetCardShirt();
            material.SetTexture(_mainTexture, cardShirt);

            Color color = new Color(0f, 0f, 0f, 0f);
            material.SetColor(_suitColor, color);
            material.SetColor(_figureTint, color);
        }
    }
}