using UnityEngine;
using Views;

namespace Game
{
    internal sealed class GameStarter : MonoBehaviour
    {
        private const int CardsCount = 6;

        private void Awake()
        {
            CreateViews();
            
            FillModels();
        }

        private void CreateViews()
        {
            new GameObject("InputView").AddComponent<InputView>();
        }

        private void FillModels()
        {

        }
    }
}