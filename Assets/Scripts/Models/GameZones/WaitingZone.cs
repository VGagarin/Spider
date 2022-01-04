using Game.Model;
using UnityEngine;

namespace Models.GameZones
{
    internal class WaitingZone : IGameZone
    {
        private readonly Transform _point;

        public CardsZone ZoneType => CardsZone.Waiting;

        public WaitingZone(Transform point)
        {
            _point = point;
        }
        
        public Transform GetPoint(int columnId = 0)
        {
            return _point;
        }
    }
}