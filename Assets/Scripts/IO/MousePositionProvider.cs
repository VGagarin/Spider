using System;
using UnityEngine;

namespace IO
{
    internal sealed class MousePositionProvider : MonoBehaviour
    {
        public event Action<Vector3> MousePositionUpdated;

        private Camera _mainCamera;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Vector3 screenToWorldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            MousePositionUpdated?.Invoke(screenToWorldPoint);
        }
    }
}