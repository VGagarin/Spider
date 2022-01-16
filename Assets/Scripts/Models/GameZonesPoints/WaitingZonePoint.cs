using Game.Model;
using UnityEngine;

namespace Models.GameZones
{
    internal class WaitingZonePoint : IGameZonePoint
    {
        private readonly Transform _point;

        public GameZoneType ZoneTypeType => GameZoneType.Waiting;

        public WaitingZonePoint(Transform point)
        {
            _point = point;
        }
        
        public Transform GetPoint(int columnId = 0)
        {
            return _point;
        }
    }
}