using Game.Model;
using UnityEngine;

namespace Models.GameZones
{
    internal class MainZone : IGameZone
    {
        private readonly Transform[] _points;
        
        public CardsZone ZoneType => CardsZone.Main;
        public Transform[] Points => _points;
        
        public MainZone(Transform[] points)
        {
            _points = points;
        }

        public Transform GetPoint(int columnId = 0)
        {
            return _points[columnId];
        }
    }
}