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
        [SerializeField] private DealingView _dealingView;

        private void Awake()
        {
            CreateViews();
            
            FillModels();
        }

        private void CreateViews()
        {
            new GameObject(nameof(InputView)).AddComponent<InputView>();
            Instantiate(_cardsView);
            Instantiate(_dealingView);
        }

        private void FillModels()
        {
            ModelRepository.GetModel<CardsModel>().CreateDeck();
        }
    }
}