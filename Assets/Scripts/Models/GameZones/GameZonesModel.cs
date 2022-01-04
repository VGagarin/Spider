using System;
using Game.Model;
using Models.Base;
using UnityEngine;

namespace Models.GameZones
{
    internal class GameZonesModel : IModel
    {
        public MainZone MainZone { get; set; }
        public WaitingZone WaitingZone { get; set; }
        public DiscardZone DiscardZone { get; set; }

        public Transform[] MainZonePoints => MainZone.Points;

        public void SetMainZonePoints(Transform[] points)
        {
            MainZone = new MainZone(points);
        }

        public void SetWaitingZonePoint(Transform point)
        {
            WaitingZone = new WaitingZone(point);
        }

        public void SetDiscardZonePoint(Transform point)
        {
            DiscardZone = new DiscardZone(point);
        }

        public IGameZone GetZoneByType(CardsZone targetZone)
        {
            switch (targetZone)
            {
                case CardsZone.Waiting:
                    return WaitingZone;
                case CardsZone.Main:
                    return MainZone;
                case CardsZone.Discard:
                    return DiscardZone;
                default:
                    throw new Exception($"Incorrect {nameof(CardsZone)} type");
            }
        }
    }
}