using DG.Tweening;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "GameSettings/" + nameof(DealingSettings), fileName = nameof(DealingSettings))]
    internal sealed class DealingSettings : ScriptableObject
    {
        public const string Path = nameof(DealingSettings);
        
        [SerializeField] private float _maxDistanceBetweenColumns = 0.55f;
        [SerializeField] private float _smallVerticalOffset = 0.2f;
        [SerializeField] private float _delayBetweenCardsDeal = 0.05f;
        [SerializeField] private float _cardSpeed = 10;
        [SerializeField] private Ease _easing = Ease.InOutSine;

        public float MaxDistanceBetweenColumns => _maxDistanceBetweenColumns;
        public float SmallVerticalOffset => _smallVerticalOffset;
        public float DelayBetweenCardsDeal => _delayBetweenCardsDeal;
        public float CardSpeed => _cardSpeed;
        public Ease Easing => _easing;
    }
}