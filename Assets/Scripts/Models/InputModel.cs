using System;
using Models.Base;
using UnityEngine;

namespace Models
{
    internal class InputModel : IModel
    {
        public event Action<Vector3> MousePositionUpdated;
        public event Action<int> CapturedCardUpdated; 
        public event Action<int> ReleasedCardUpdated; 

        public Vector3 MousePosition { get; private set; }
        public int? CapturedCardId { get; private set;}
        public int? ReleasedCardId { get; private set;}

        public void SetMousePosition(Vector3 mousePosition)
        {
            if (MousePosition == mousePosition)
                return;
            
            MousePosition = mousePosition;
            MousePositionUpdated?.Invoke(mousePosition);
        }

        public void SetCapturedCardId(int id)
        {
            CapturedCardId = id;
            CapturedCardUpdated?.Invoke(id);
        }

        public void SetReleasedCardId(int id)
        {
            CapturedCardId = null;
            ReleasedCardId = id;
            ReleasedCardUpdated?.Invoke(id);
        }
    }
}