using Models;
using Models.Base;
using UnityEngine;
using Views;
using Views.Cards;

namespace Game
{
    internal sealed class GameStarter : MonoBehaviour
    {
        private const int CardsCount = 6;

        [SerializeField] private CardsView _cardsView;
        [SerializeField] private GameZonesView _gameZonesView;

        private void Awake()
        {
            CreateViews();
            
            FillModels();
        }

        private void CreateViews()
        {
            new GameObject(nameof(InputView)).AddComponent<InputView>();
            Instantiate(_cardsView);
            Instantiate(_gameZonesView);
            new GameObject(nameof(DealingView)).AddComponent<DealingView>();
        }

        private void FillModels()
        {
            ModelRepository.GetModel<CardsModel>().CreateDeck();
        }
    }
}