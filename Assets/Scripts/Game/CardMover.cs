using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using GeneralPurpose;
using UnityEngine;

namespace Game
{
    internal class CardMover
    {
        private readonly float _cardSpeed;
        private readonly Ease _easing;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public CardMover(float cardSpeed, Ease easing)
        {
            _cardSpeed = cardSpeed;
            _easing = easing;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async void MoveToLocalPositionAfterDelay(float delay, Vector3 target, Transform card, Action endCallback,
            InsertAction insertAction = null)
        {
            try
            {
                Vector3 cardLocalPosition = card.localPosition;
                
                await Task.Delay(TimeSpan.FromSeconds(delay), _cancellationTokenSource.Token);
                
                float duration = Vector3.Distance(target, cardLocalPosition) / _cardSpeed;
                
                Sequence sequence = DOTween.Sequence();
                sequence
                    .Append(card
                        .DOLocalMove(target, duration)
                        .SetEase(_easing))
                    .AppendCallback(() =>
                    {
                        endCallback?.Invoke();
                    });

                if (insertAction != null)
                {
                    TimeSpan spanBeforeAction = TimeSpan.FromSeconds(duration * insertAction.RelativeTime);
                    await Task.Delay(spanBeforeAction, _cancellationTokenSource.Token);
                
                    insertAction.Action?.Invoke();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        ~CardMover() => _cancellationTokenSource?.Cancel();
    }
}