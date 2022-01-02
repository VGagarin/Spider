using Game;
using Game.Model;
using Models;
using Models.Base;
using UnityEngine;
using ViewModels.Base;

namespace ViewModels
{
    internal class DealingViewModel : BaseViewModel
    {
        public Vector3 MainZonePosition { get; set; }
        
        public DealingViewModel()
        {
            ModelRepository.GetModel<CardsModel>().DeckCreated += OnDeckCreated;
        }

        private void OnDeckCreated(Deck deck)
        {
            Deal(deck, MainZonePosition);
        }

        private void Deal(Deck deck, Vector3 mainZonePosition)
        {
            const int cardsToDeal = 54;
            const int cardsToOpen = 10;
            const int columns = 10;
            const float distanceBetweenColumns = 0.55f;
            const float smallVerticalOffset = 0.2f;
            const float delayBetweenCardsWhenDealing = 0.05f;
            
            Card[] cards = deck.Cards;

            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();

            for (int i = 0; i < cardsToDeal; i++)
            {
                int columnIndex = i % columns;
                int rowIndex = i / columns;

                float xOffset = columnIndex * distanceBetweenColumns;
                float yOffset = -rowIndex * smallVerticalOffset;
                Vector3 offset = new Vector3(xOffset, yOffset, 0);

                float delay = delayBetweenCardsWhenDealing * i;

                CardMoveData moveData = new CardMoveData
                {
                    CardToMove = cards[i],
                    DelayBeforeMove = delay,
                    TargetPosition = mainZonePosition + offset,
                    TargetLayer = rowIndex,
                    TargetZone = CardsZone.Main,
                    ColumnIndex = columnIndex
                };

                if (cardsToDeal - i <= cardsToOpen)
                    moveData.TargetStateIsOpen = true;

                cardsModel.MoveCard(moveData);
            }
        }
        
        ~DealingViewModel()
        {
            ModelRepository.GetModel<CardsModel>().DeckCreated -= OnDeckCreated;
        }
    }
}