using UnityEngine;
using UnityEngine.Events;

namespace IO
{
    internal sealed class MousePositionProvider : MonoBehaviour
    {
        public UnityEvent<Vector3> MousePositionUpdated = new UnityEvent<Vector3>();

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