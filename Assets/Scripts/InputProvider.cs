using System.Linq;
using UnityEngine;
using Views;

namespace Spider
{
    internal sealed class InputProvider : MonoBehaviour
    {
        private Transform _attachedView;
        private Camera _mainCamera;
        private Vector3 _offset;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 origin = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero);
                CardView[] views = hits
                    .Select(hit => hit.collider.GetComponent<CardView>())
                    .Where(view => view != null)
                    .ToArray();

                if (views.Any())
                {
                    CardView cardView = views.OrderBy(view => view.Layer).Last();

                    if (cardView)
                    {
                        _attachedView = cardView.transform;
                        _offset = _attachedView.position - origin;
                        _offset.z = 0;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && _attachedView)
            {
                _attachedView = null;
            }

            if (_attachedView)
            {
                Vector3 screenToWorldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                screenToWorldPoint.z = _attachedView.position.z;
                _attachedView.position = screenToWorldPoint + _offset;
            }
        }
    }
}