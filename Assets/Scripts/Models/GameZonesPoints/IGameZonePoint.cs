using Game.Model;
using UnityEngine;

namespace Models.GameZones
{
    internal interface IGameZonePoint
    {
        GameZoneType ZoneTypeType { get; }

        Transform GetPoint(int columnId = 0);
    }
}