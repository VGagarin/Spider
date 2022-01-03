using System;
using Game.Model;
using Models.Base;
using UnityEngine;

namespace Models
{
    internal class InputModel : IModel
    {
        public event Action<Vector3> MousePositionUpdated;
        public event Action<CardInputData> CapturedCardUpdated; 
        public event Action<CardInputData> ReleasedCardUpdated; 

        public Vector3 MousePosition { get; private set; }

        public void SetMousePosition(Vector3 mousePosition)
        {
            if (MousePosition == mousePosition)
                return;
            
            MousePosition = mousePosition;
            MousePositionUpdated?.Invoke(mousePosition);
        }

        public void SetCapturedCardId(CardInputData inputData)
        {
            CapturedCardUpdated?.Invoke(inputData);
        }

        public void SetReleasedCardId(CardInputData inputData)
        {
            ReleasedCardUpdated?.Invoke(inputData);
        }
    }
}