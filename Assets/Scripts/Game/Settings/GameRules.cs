using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "GameSettings/" + nameof(GameRules), fileName = nameof(GameRules))]
    internal sealed class GameRules : ScriptableObject
    {
        public const string Path = nameof(GameRules);
        
        [SerializeField] private int _cardsInDeck = 104;
        [SerializeField] private int _columns = 10;
        [SerializeField] private int _cardsToOpen = 10;
        [SerializeField] private int _cardsToDeal = 54;
        [SerializeField] private int _cardsToAdditionalDeal = 10;
        
        public int CardsInDeck => _cardsInDeck;
        public int Columns => _columns;
        public int CardsToOpen => _cardsToOpen;
        public int CardsToDeal => _cardsToDeal;
        public int CardsToAdditionalDeal => _cardsToAdditionalDeal;
    }
}