using System;
using Game.Model;
using Models.Base;
using UnityEngine;

namespace Models.GameZones
{
    internal class GameZonesPointsModel : IModel
    {
        public MainZonePoint MainZonePoint { get; set; }
        public WaitingZonePoint WaitingZonePoint { get; set; }
        public DiscardZonePoint DiscardZonePoint { get; set; }

        public Transform[] MainZonePoints => MainZonePoint.Points;

        public void SetMainZonePoints(Transform[] points)
        {
            MainZonePoint = new MainZonePoint(points);
        }

        public void SetWaitingZonePoint(Transform point)
        {
            WaitingZonePoint = new WaitingZonePoint(point);
        }

        public void SetDiscardZonePoint(Transform point)
        {
            DiscardZonePoint = new DiscardZonePoint(point);
        }

        public IGameZonePoint GetZoneByType(GameZoneType targetZoneType)
        {
            switch (targetZoneType)
            {
                case GameZoneType.Waiting:
                    return WaitingZonePoint;
                case GameZoneType.Main:
                    return MainZonePoint;
                case GameZoneType.Discard:
                    return DiscardZonePoint;
                default:
                    throw new Exception($"Incorrect {nameof(GameZoneType)} type");
            }
        }
    }
}