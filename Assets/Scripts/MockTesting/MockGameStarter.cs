using System.Collections.Generic;
using Game;
using Game.Model;
using Models;
using Models.Base;

namespace MockTesting
{
    internal class MockGameStarter : GameStarter
    {
        protected override void FillModels()
        {
            List<Suit> suits = new List<Suit> {Suit.Hearts};
            Deck deck = new Deck(suits, false);

            ModelRepository.GetModel<CardsModel>().SetDeck(deck);
        }
    }
}