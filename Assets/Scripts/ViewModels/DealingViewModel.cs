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
        private readonly CardsModel _cardsModel;

        public Transform[] ColumnPoints { get; set; }
        
        public DealingViewModel()
        {
            _cardsModel = ModelRepository.GetModel<CardsModel>();
            _cardsModel.DeckCreated += OnDeckCreated;
            
            _dealingSettings = SpiderSettings.DealingSettings;
            _gameRules = SpiderSettings.GameRules;
        }

        private void OnDeckCreated(Deck deck)
        {
            _cardsModel.UpdateColumnPoints(ColumnPoints);
            Deal(deck);
        }

        private void Deal(Deck deck)
        {
            Card[] cards = deck.Cards;

            CardsModel cardsModel = ModelRepository.GetModel<CardsModel>();

            for (int i = 0; i < _gameRules.CardsToDeal; i++)
            {
                int columnIndex = i % _gameRules.Columns;
                int rowIndex = i / _gameRules.Columns;

                float yOffset = -rowIndex * _dealingSettings.SmallVerticalOffset;

                float delay = _dealingSettings.DelayBetweenCardsDeal * i;

                CardMoveData moveData = new CardMoveData
                {
                    CardToMove = cards[i],
                    DelayBeforeMove = delay,
                    TargetPosition = Vector3.up * yOffset,
                    TargetLayer = rowIndex,
                    TargetZone = CardsZone.Main,
                    ColumnIndex = columnIndex,
                    TargetParent = ColumnPoints[columnIndex],
                    IsLocalMove = true
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