using System;
using System.Linq;
using UnityEngine;
using Views.Cards;

namespace IO
{
    internal sealed class CardsInputProvider : MonoBehaviour
    {
        private CardSubview _attachedView;
        private Camera _mainCamera;

        public event Action<int, Vector3> CardCaptured;
        public event Action<int, Vector3> CardReleased;
        
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
                CardSubview[] views = hits
                    .Select(hit => hit.collider.GetComponent<CardSubview>())
                    .Where(view => view != null)
                    .ToArray();

                if (views.Any())
                {
                    CardSubview cardSubview = views.OrderBy(view => view.Layer).Last();

                    if (cardSubview && cardSubview.IsMovable)
                    {
                        CardCaptured?.Invoke(cardSubview.CardId, cardSubview.transform.position);
                        
                        _attachedView = cardSubview;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && _attachedView)
            {
                CardReleased?.Invoke(_attachedView.CardId, _attachedView.transform.position);
                
                _attachedView = null;
            }
        }
    }
}