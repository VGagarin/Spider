using Game.Model;
using UnityEngine;

namespace Models.GameZones
{
    internal class DiscardZonePoint : IGameZonePoint
    {
        private readonly Transform _point;

        public GameZoneType ZoneTypeType => GameZoneType.Discard;
        
        public DiscardZonePoint(Transform point)
        {
            _point = point;
        }
        
        public Transform GetPoint(int columnId = 0)
        {
            return _point;
        }
    }
}