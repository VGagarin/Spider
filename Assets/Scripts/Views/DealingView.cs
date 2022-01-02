using System;
using System.Linq;
using Game.Settings;
using UnityEngine;
using ViewModels;
using Views.Base;

namespace Views
{
    internal class DealingView : BaseView<DealingViewModel>
    {
        [SerializeField] private Transform _mainZone;

        private Transform[] _controlPoints;
        private Transform _transform;
        
        protected override void Initialize()
        {
            _transform = transform;
            
            _viewModel.ControlPoints = CreateControlPoints();
            
            UpdatePositions();
            
            Vector3 mainZonePosition = _mainZone.position;
            mainZonePosition.x = _controlPoints.First().position.x;
            _viewModel.MainZonePosition = mainZonePosition;
        }

        private void Update()
        {
            UpdatePositions();
        }

        private Transform[] CreateControlPoints()
        {
            _controlPoints = new Transform[SpiderSettings.GameRules.Columns];

            for (int i = 0; i < _controlPoints.Length; i++)
            {
                GameObject controlPoint = new GameObject($"ControlPoint {i}");

                Transform pointTransform = controlPoint.transform;
                pointTransform.parent = transform;

                _controlPoints[i] = pointTransform;
            }

            return _controlPoints;
        }

        private void UpdatePositions()
        {
            Vector2 screenBounds = FindScreenBounds();
            DealingSettings dealingSettings = SpiderSettings.DealingSettings;
            
            float offsetFromEdgesOfScreen = dealingSettings.OffsetFromEdgesOfScreen;

            Vector3 start = _mainZone.position;
            start.x = screenBounds.x + offsetFromEdgesOfScreen;
            
            Vector3 end = start;
            end.x = screenBounds.y - offsetFromEdgesOfScreen;

            int pointsCount = _controlPoints.Length;
            
            float distanceBetweenColumns = (end.x - start.x) / (pointsCount - 1);
            float maxDistanceBetweenColumns = SpiderSettings.DealingSettings.MaxDistanceBetweenColumns;
            if (distanceBetweenColumns > maxDistanceBetweenColumns)
            {
                float halfDelta = (distanceBetweenColumns - maxDistanceBetweenColumns) / 2f * (pointsCount - 1);
                start.x += halfDelta;
                end.x -= halfDelta;
            }
            
            for (int i = 0; i < pointsCount; i++)
            {
                _controlPoints[i].transform.position = Vector3.Lerp(start, end, (float)i / (pointsCount - 1));
            }
        }
        
        private Vector2 FindScreenBounds()
        {
            Camera mainCamera = Camera.main;

            if (mainCamera == null)
                throw new Exception("Camera not found");

            Vector3 leftPosition = mainCamera.ScreenToWorldPoint(new Vector3 (0f, 0f, 0));
            Vector3 rightPosition = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, 0f, 0));
            
            return new Vector2(leftPosition.x, rightPosition.x);
        }
    }
}