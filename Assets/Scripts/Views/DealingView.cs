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
        
        protected override void Initialize()
        {
            _viewModel.ControlPoints = CreateControlPoints();
            
            UpdatePositions();
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
            Vector2 bounds = FindBounds();
            
            Vector3 start = _mainZone.position;
            start.x = bounds.x;
            
            Vector3 end = start;
            end.x = bounds.y;
            
            for (int i = 0; i < _controlPoints.Length; i++)
            {
                _controlPoints[i].transform.position = Vector3.Lerp(start, end, (float)i / (_controlPoints.Length - 1));
            }
        }
        
        private Vector2 FindBounds()
        {
            Vector2 bounds = FindScreenBounds();
            
            float offsetFromEdgesOfScreen = SpiderSettings.DealingSettings.OffsetFromEdgesOfScreen;
            bounds.x += offsetFromEdgesOfScreen;
            bounds.y -= offsetFromEdgesOfScreen;

            int pointsCount = _controlPoints.Length;
            
            float distanceBetweenColumns = (bounds.y - bounds.x) / (pointsCount - 1);
            float maxDistanceBetweenColumns = SpiderSettings.DealingSettings.MaxDistanceBetweenColumns;
            if (distanceBetweenColumns > maxDistanceBetweenColumns)
            {
                float halfDelta = (distanceBetweenColumns - maxDistanceBetweenColumns) / 2f * (pointsCount - 1);
                bounds.x += halfDelta;
                bounds.y -= halfDelta;
            }
            
            return bounds;
        }

        private Vector2 FindScreenBounds()
        {
            Camera mainCamera = Camera.main;

            if (mainCamera == null)
                throw new Exception("Camera not found");

            float left = mainCamera.ScreenToWorldPoint(new Vector3 (0f, 0f, 0)).x;
            float right = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, 0f, 0)).x;

            return new Vector2(left, right);
        }
    }
}