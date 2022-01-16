using Game.Model;
using UnityEngine;

namespace Models.GameZones
{
    internal class MainZonePoint : IGameZonePoint
    {
        private readonly Transform[] _points;
        
        public GameZoneType ZoneTypeType => GameZoneType.Main;
        public Transform[] Points => _points;
        
        public MainZonePoint(Transform[] points)
        {
            _points = points;
        }

        public Transform GetPoint(int columnId = 0)
        {
            return _points[columnId];
        }
    }
}