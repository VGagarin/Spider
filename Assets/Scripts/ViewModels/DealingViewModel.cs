using System.Collections.Generic;
using Game;
using Game.Model;
using Game.Settings;
using Models;
using Models.Base;
using Models.Cards;
using UnityEngine;
using ViewModels.Base;

namespace ViewModels
{
    internal class DealingViewModel : BaseViewModel
    {
        private readonly DealingSettings _dealingSettings;
        private readonly GameRules _gameRules;
        private readonly CardsModel _cardsModel;
        private readonly InputModel _inputModel;

        public DealingViewModel()
        {
            _cardsModel = ModelRepository.GetModel<CardsModel>();
            _inputModel = ModelRepository.GetModel<InputModel>();

            _cardsModel.DeckCreated += OnDeckCreated;
            _inputModel.WaitingZoneActivated += DealAdditionalCards;
            
            _dealingSettings = SpiderSettings.DealingSettings;
            _gameRules = SpiderSettings.GameRules;
        }

        private void OnDeckCreated(Deck deck)
        {
            Deal(deck);
        }

        private void Deal(Deck deck)
        {
            Card[] cards = deck.Cards;
            
            for (int i = 0; i < _gameRules.CardsToDeal; i++)
            {
                int columnId = i % _gameRules.Columns;
                float delay = _dealingSettings.DelayBetweenCardsDeal * i;
                
                BaseCardMoveData baseMoveData = CreateBaseMoveData(cards[i].Id, delay, columnId);
                
                _cardsModel.MoveCard(baseMoveData);
            }
        }
        
        private void DealAdditionalCards()
        {
            List<Card> cardsInWaiting = _cardsModel.GetCardsInWaitingZone();
            List<Card> cardsToDeal = cardsInWaiting.GetRange(0, _gameRules.CardsToAdditionalDeal);

            for (int i = 0; i < cardsToDeal.Count; i++)
            {
                int columnId = i % _gameRules.Columns;
                float delay = _dealingSettings.DelayBetweenCardsDeal * i;

                BaseCardMoveData baseMoveData = CreateBaseMoveData(cardsToDeal[i].Id, delay, columnId);
                _cardsModel.MoveCard(baseMoveData);
            }
        }

        private BaseCardMoveData CreateBaseMoveData(int cardId, float delay, int columnId)
        {
            return new BaseCardMoveData
            {
                CardId = cardId,
                DelayBeforeMove = delay,
                ColumnId = columnId,
                SourceZoneType = GameZoneType.Waiting,
                TargetZoneType = GameZoneType.Main,
            };
        }

        ~DealingViewModel()
        {
            _cardsModel.DeckCreated -= OnDeckCreated;
            _inputModel.WaitingZoneActivated -= DealAdditionalCards;
        }
    }
}