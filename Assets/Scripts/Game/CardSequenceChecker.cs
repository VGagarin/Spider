using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model;

namespace Game
{
    internal static class CardSequenceChecker
    {
        private static readonly Value[] Values = (Value[]) Enum.GetValues(typeof(Value));

        public static bool HasEndedSequenceCollected(List<Card> column, out List<Card> potentialEndedSequence)
        {
            potentialEndedSequence = null;

            (Value lastValue, Suit suit) = column.Last();
            if (lastValue == Value.Ace && column.Count >= Values.Length)
            {
                potentialEndedSequence = column.GetRange(column.Count - Values.Length, Values.Length);

                for (int i = 0; i < Values.Length; i++)
                {
                    Card card = potentialEndedSequence[i];
                    if (card.Suit != suit || card.Value != Values[i] || !card.IsOpen)
                        return false;
                }

                return true;
            }

            return false;
        }

        public static bool CardCanBeCaptured(List<Card> column, int row)
        {
            bool cardIsUpper = row == column.Count - 1;
            if (cardIsUpper)
                return true;
            
            for (int i = row + 1; i < column.Count; i++)
            {
                bool isValuesNotAttached = column[i - 1].Value != column[i].Value - 1;
                bool isSuitsNotEqual = column[i - 1].Suit != column[i].Suit;
                if (isValuesNotAttached || isSuitsNotEqual)
                    return false;
            }

            return true;
        }

        public static bool IsTurnAvailable(Card cardToMove, Card target) => cardToMove.Value == target.Value + 1;
    }
}