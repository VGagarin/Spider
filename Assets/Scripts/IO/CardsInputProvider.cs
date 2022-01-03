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

        public event Action<int, Vector3> CardTryCaptured;
        public event Action<int, Vector3> CardReleased;
        public event Action<int> ClosedCardClicking;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && TryFindSubviewUnderMouse(out CardSubview cardSubview))
            {
                ProcessButtonDown(cardSubview);
            }

            if (Input.GetMouseButtonUp(0) && _attachedView != null)
            {
                ProcessButtonUp();
            }
        }

        public void CardCapturedFailed() => _attachedView = null;

        private bool TryFindSubviewUnderMouse(out CardSubview cardSubview)
        {
            cardSubview = null;
            
            Vector3 origin = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.zero);
            CardSubview[] views = hits
                .Select(hit => hit.collider.GetComponent<CardSubview>())
                .Where(view => view != null)
                .ToArray();

            if (!views.Any()) 
                return false;
            
            cardSubview = views.OrderBy(view => view.Layer).Last();
            return true;
        }
            
        private void ProcessButtonDown(CardSubview subview)
        {
            if (subview.IsMovable)
            {
                _attachedView = subview;
                CardTryCaptured?.Invoke(subview.CardId, subview.transform.position);
            }
            else
            {
                ClosedCardClicking?.Invoke(subview.CardId);
            }
        }

        private void ProcessButtonUp()
        {
            CardReleased?.Invoke(_attachedView.CardId, _attachedView.transform.position);
                
            _attachedView = null;
        }
    }
}