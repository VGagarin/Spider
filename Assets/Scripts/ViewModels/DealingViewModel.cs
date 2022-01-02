using Game;
using Game.Model;
using Game.Settings;
using Models;
using Models.Base;
using UnityEngine;
using ViewModels.Base;

namespace ViewModels
{
    internal class DealingViewModel : BaseViewModel
    {
        private readonly DealingSettings _dealingSettings;
        private readonly GameRules _gameRules;
        
        public Vector3 MainZonePosition { get; set; }
        
        public DealingViewModel()
        {
            ModelRepository.GetModel<CardsModel>().DeckCreated += OnDeckCreated;
            
            _dealingSettings = SpiderSettings.DealingSettings;
            _gameRules = SpiderSettings.GameRules;
        }

        private void OnDeckCreated(Deck deck)
        {
            Deal(deck, MainZonePosition);
        }

        private void Deal(Deck deck, Vector3 mainZonePosition)
        {
            Card[] cards = deck.Cards;

            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();

            for (int i = 0; i < _gameRules.CardsToDeal; i++)
            {
                int columnIndex = i % _gameRules.Columns;
                int rowIndex = i / _gameRules.Columns;

                float xOffset = columnIndex * _dealingSettings.DistanceBetweenColumns;
                float yOffset = -rowIndex * _dealingSettings.SmallVerticalOffset;
                Vector3 offset = new Vector3(xOffset, yOffset, 0);

                float delay = _dealingSettings.DelayBetweenCardsDeal * i;

                CardMoveData moveData = new CardMoveData
                {
                    CardToMove = cards[i],
                    DelayBeforeMove = delay,
                    TargetPosition = mainZonePosition + offset,
                    TargetLayer = rowIndex,
                    TargetZone = CardsZone.Main,
                    ColumnIndex = columnIndex
                };

                if (_gameRules.CardsToDeal - i <= _gameRules.CardsToOpen)
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