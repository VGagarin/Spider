using System.Collections.Generic;
using Game;
using Game.Settings;
using GeneralPurpose;
using UnityEngine;

namespace Views.Cards
{
    internal class CardsDragger
    {
        private readonly Camera _mainCamera;
        private readonly CardMover _cardMover;
        private readonly List<AttachData> _attaches = new List<AttachData>();

        public CardsDragger(Camera mainCamera, CardMover cardMover)
        {
            _mainCamera = mainCamera;
            _cardMover = cardMover;
        }
        
        public void AttachSubview(CardSubview subview)
        {
            subview.Layer += SpiderSettings.GameRules.CardsInDeck;
                
            Transform transformToAttach = subview.transform;
            Vector3 startLocalPosition = transformToAttach.localPosition;
            Vector3 offset = transformToAttach.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            offset.z = 0;

            AttachData attachData = new AttachData
            {
                Subview = subview,
                StartLocalPosition = startLocalPosition,
                Offset = offset
            };
            
            _attaches.Add(attachData);
        }

        public void ReturnAllAttaches()
        {
            foreach (AttachData attachData in _attaches)
            {
                ReturnToStartPosition(attachData);
            }
            
            _attaches.Clear();
        }
        
        public void UpdatePositions(Vector3 mousePosition)
        {
            foreach (AttachData attachData in _attaches)
            {
                UpdatePosition(attachData, mousePosition);
            }
        }
        
        public void Clear() => _attaches.Clear();

        private void ReturnToStartPosition(AttachData attachData)
        {
            InsertAction insertAction = new InsertAction
            {
                Action = () => attachData.Subview.Layer -= SpiderSettings.GameRules.CardsInDeck,
                RelativeTime = 1f
            };

            Vector3 position = attachData.StartLocalPosition;
            
            _cardMover.MoveToLocalPositionAfterDelay(0, position, attachData.Subview.transform, insertAction);
        }

        private void UpdatePosition(AttachData attachData, Vector3 mousePosition)
        {
            Transform attachedTransform = attachData.Subview.transform;
            mousePosition.z = attachedTransform.position.z;
            attachedTransform.position = mousePosition + attachData.Offset;
        }
    }
}