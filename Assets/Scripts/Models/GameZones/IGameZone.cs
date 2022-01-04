using Game.Model;
using UnityEngine;

namespace Models.GameZones
{
    internal interface IGameZone
    {
        CardsZone ZoneType { get; }

        Transform GetPoint(int columnId = 0);
    }
}