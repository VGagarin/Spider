using Game.Model;
using UnityEngine;

namespace Models.GameZones
{
    internal class DiscardZone : IGameZone
    {
        private readonly Transform _point;

        public CardsZone ZoneType => CardsZone.Discard;
        
        public DiscardZone(Transform point)
        {
            _point = point;
        }
        
        public Transform GetPoint(int columnId = 0)
        {
            return _point;
        }
    }
}