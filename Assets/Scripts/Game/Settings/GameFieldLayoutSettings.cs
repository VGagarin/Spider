using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "GameSettings/" + nameof(GameFieldLayoutSettings),
                     fileName = nameof(GameFieldLayoutSettings))]
    internal sealed class GameFieldLayoutSettings : ScriptableObject
    {
        public const string Path = nameof(GameFieldLayoutSettings);
        
        [SerializeField] private float _smallVerticalOffset = 0.2f;
        
        [Header("AdaptiveLayout")]
        [SerializeField] private float _maxDistanceBetweenColumns = 0.55f;
        [SerializeField, Range(0, 1)] private float _leftPaddingForDiscard = 0.25f;
        [SerializeField, Range(0, 1)] private float _rightPaddingForWaiting = 0.25f;
        [SerializeField, Range(0, 1)] private float _bottomPaddingForDiscard = 0.25f;
        [SerializeField, Range(0, 1)] private float _bottomPaddingForWaiting = 0.25f;

        public float SmallVerticalOffset => _smallVerticalOffset;
        public float MaxDistanceBetweenColumns => _maxDistanceBetweenColumns;

        public float LeftPaddingForDiscard => _leftPaddingForDiscard;
        public float RightPaddingForWaiting => _rightPaddingForWaiting;
        public float BottomPaddingForDiscard => _bottomPaddingForDiscard;
        public float BottomPaddingForWaiting => _bottomPaddingForWaiting;
    }
}