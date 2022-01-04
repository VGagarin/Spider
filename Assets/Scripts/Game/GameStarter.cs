using Game.Settings;
using Models;
using Models.Base;
using UnityEngine;
using Views;
using Views.Cards;

namespace Game
{
    internal class GameStarter : MonoBehaviour
    {
        private const int CardsCount = 6;

        [SerializeField] private CardsView _cardsView;
        [SerializeField] private GameZonesView _gameZonesView;

        private void Awake()
        {
            CreateViews();
            
            FillModels();
        }

        protected virtual void CreateViews()
        {
            new GameObject(nameof(InputView)).AddComponent<InputView>();
            Instantiate(_cardsView);
            Instantiate(_gameZonesView);
            new GameObject(nameof(DealingView)).AddComponent<DealingView>();
        }

        protected virtual void FillModels()
        {
            Deck deck = new Deck(SpiderSettings.GameRules.Suits);
            ModelRepository.GetModel<CardsModel>().SetDeck(deck);
        }
    }
}