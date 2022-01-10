using System;
using Game.Settings;
using UnityEngine;
using ViewModels;
using Views.Base;

namespace Views
{
    internal class GameZonesView : BaseView<GameZonesViewModel>
    {
        [SerializeField] private Transform _waitingZonePoint;
        [SerializeField] private Transform _mainZonePoint;
        [SerializeField] private Transform _discardZonePoint;

        private Transform[] _columnPoints;
        
        protected override void Initialize()
        {
            _viewModel.SetWaitingZonePoint(_waitingZonePoint);
            _viewModel.SetColumnPoints(CreateColumnPoints());
            _viewModel.SetDiscardZonePoint(_discardZonePoint);
            
            UpdatePositions();
        }

        private void Update()
        {
            UpdatePositions();
        }

        private Transform[] CreateColumnPoints()
        {
            _columnPoints = new Transform[SpiderSettings.GameRules.Columns];

            for (int i = 0; i < _columnPoints.Length; i++)
            {
                GameObject controlPoint = new GameObject($"ColumnPoint {i}");

                Transform pointTransform = controlPoint.transform;
                pointTransform.parent = transform;

                _columnPoints[i] = pointTransform;
            }

            return _columnPoints;
        }

        private void UpdatePositions()
        {
            Rect screenBounds = FindScreenBounds();
            Vector2 bounds = FindMainZoneBounds(screenBounds);
            
            UpdateMainZonePositions(bounds);
            UpdateDiscardPosition(screenBounds);
            UpdateWaitingPosition(screenBounds);
        }

        private void UpdateMainZonePositions(Vector2 bounds)
        {
            Vector3 start = _mainZonePoint.position;
            start.x = bounds.x;
            Vector3 end = start;
            end.x = bounds.y;
            
            for (int i = 0; i < _columnPoints.Length; i++)
            {
                _columnPoints[i].transform.position = Vector3.Lerp(start, end, (float)i / (_columnPoints.Length - 1));
            }
        }

        private void UpdateDiscardPosition(Rect bounds)
        {
            float leftPaddingRatio = SpiderSettings.GameFieldLayoutSettings.LeftPaddingForDiscard;
            float bottomPaddingRatio = SpiderSettings.GameFieldLayoutSettings.BottomPaddingForDiscard;

            Vector3 position = _discardZonePoint.position;
            
            position.x = Mathf.Lerp(bounds.xMin, bounds.xMax, leftPaddingRatio);
            position.y = Mathf.Lerp(bounds.yMin, bounds.yMax, 1f - bottomPaddingRatio);

            _discardZonePoint.position = position;
        }

        private void UpdateWaitingPosition(Rect bounds)
        {
            float rightPaddingRatio = SpiderSettings.GameFieldLayoutSettings.RightPaddingForWaiting;
            float bottomPaddingRatio = SpiderSettings.GameFieldLayoutSettings.BottomPaddingForWaiting;
            
            Vector3 position = _waitingZonePoint.position;
            
            position.x = Mathf.Lerp(bounds.xMin, bounds.xMax, 1f - rightPaddingRatio);
            position.y = Mathf.Lerp(bounds.yMin, bounds.yMax, 1f - bottomPaddingRatio);

            _waitingZonePoint.position = position;
        }

        private Vector2 FindMainZoneBounds(Rect screenBounds)
        {
            Vector2 horizontalBounds = new Vector2(screenBounds.xMin, screenBounds.xMax);
            int pointsCount = _columnPoints.Length;
            
            float distanceBetweenColumns = (horizontalBounds.y - horizontalBounds.x) / (pointsCount + 1);
            
            horizontalBounds.x += distanceBetweenColumns / 1.8f;
            horizontalBounds.y -= distanceBetweenColumns / 1.8f;
            
            float maxDistanceBetweenColumns = SpiderSettings.GameFieldLayoutSettings.MaxDistanceBetweenColumns;
            if (distanceBetweenColumns > maxDistanceBetweenColumns)
            {
                float halfDelta = (distanceBetweenColumns - maxDistanceBetweenColumns) / 2f * (pointsCount + 1);
                horizontalBounds.x += halfDelta;
                horizontalBounds.y -= halfDelta;
            }
            
            return horizontalBounds;
        }

        private Rect FindScreenBounds()
        {
            Camera mainCamera = Camera.main;

            if (mainCamera == null)
                throw new Exception("Camera not found");

            float left = mainCamera.ScreenToWorldPoint(new Vector3 (0f, 0f, 0f)).x;
            float right = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, 0f, 0f)).x;
            float up = mainCamera.ScreenToWorldPoint(new Vector3(0f, mainCamera.pixelHeight, 0f)).y;
            float down = mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0)).y;
            
            return new Rect(left, up, right - left, down - up);
        }
    }
}